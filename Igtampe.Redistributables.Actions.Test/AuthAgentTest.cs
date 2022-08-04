using Igtampe.Actions;
using Igtampe.Actions.Exceptions;
using Igtampe.ChopoAuth;
using Igtampe.ChopoAuth.Exceptions;
using Igtampe.ChopoSessionManager;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Igtampe.Redistributables.Actions.Test {
    public class AuthAgentTest {

        private static readonly User[] Users = {
            new() { Username = "Chopo", IsAdmin = true, Name = "Chopo" },
            new() { Username = "Chapo", Name = "Chapo" },
            new() { Username = "Chepo", Name = "Chepo" },
            new() { Username = "Chipo", Name = "Chipo" },
            new() { Username = "Chupo", Name = "Chupo" },
        };

        private static readonly string[] Passwords = {
            "ALongPassword#123",
            "Ashorterone",
            "NumbersWooo!123020303014001230401302",
            "##)#@@!)@$%",
            "qwertyuiop[]"
        };

        private readonly ISessionManager Manager = SessionManager.Manager;
        private readonly AuthAgent<TestContext> Agent;
        private TestContext Context;

        public AuthAgentTest() {
            Context = SetupContext();            

            Agent = new(Context, Manager);
        }

        private TestContext SetupContext() { 
            var Factory = new TestContextFactory(nameof(AuthAgentTest));
            Context = Factory.Create();
            
            Manager.Reset();
            FillContext();
            
            return Context;
        }

        private void FillContext() {
            Context.AddRange(Users);
            Context.SaveChanges();
        }

        private void EmptyContext() {
            Context.RemoveRange(Context.User);
            Context.SaveChanges();
        }

        private void ResetContext() {
            EmptyContext();

            Users[0].UpdatePass(Passwords[0]);
            Users[1].UpdatePass(Passwords[1]);
            Users[2].UpdatePass(Passwords[2]);
            Users[3].UpdatePass(Passwords[3]);
            Users[4].UpdatePass(Passwords[4]);

            Users[0].IsAdmin = true;
            Users[1].IsAdmin = false;
            Users[2].IsAdmin = false;
            Users[3].IsAdmin = false;
            Users[4].IsAdmin = false;

            FillContext();
        }

        [SetUp]
        public void BeforeEach() {
            ResetContext();
            Manager.Reset();
        }

        [Test]
        public async Task GetAllDirectoryTest() {
            var Dir = await Agent.GetDirectory();
            Assert.AreEqual(5, Dir.Count, "Not all the users were returned");
        }

        [Test]
        public async Task GetSomeDirectoryTest() {
            var Dir = await Agent.GetDirectory(null,2,0);
            Assert.AreEqual(2, Dir.Count, "User array was not trimmed");
        }

        [Test]
        public async Task GetSearchDirectoryTest() {
            var Dir = await Agent.GetDirectory(Users[0].Username[..4]);
            Assert.AreEqual(1, Dir.Count, "There's more than one chopo");
            Assert.AreEqual(Users[0], Dir[0], "Returned user was not a chopo");
        }

        [Test]
        public async Task GetMeTest() {
            Guid SessionID = Manager.LogIn(Users[0].Username);
            var U = await Agent.GetMe(SessionID);
            Assert.AreEqual(Users[0], U, "User was not a Chopo");
        }

        [Test]
        public async Task GetUserTest() {
            var U = await Agent.GetUser(Users[0].Username);
            Assert.AreEqual(Users[0], U);
        }

        [Test]
        public async Task UserIsAdminTest() {
            Assert.IsTrue(await Agent.UserIsAdmin(Users[0].Username), "Chopo is not an admin");
            Assert.IsFalse(await Agent.UserIsAdmin(Users[1].Username), "Chapo is an admin");
        }

        [Test]
        public async Task SessionIsAdminTest() {
            Guid Admin = Manager.LogIn(Users[0].Username);
            Guid NotAdmin = Manager.LogIn(Users[1].Username);
            Assert.IsTrue(await Agent.SessionIsAdmin(Admin), "Chopo is not an admin");
            Assert.IsFalse(await Agent.SessionIsAdmin(NotAdmin), "Chapo is an admin");
        }

        [Test]
        public void ChangePasswordFailTest() =>
            Assert.ThrowsAsync<PasswordIncorrectException>(async () => await Agent.ChangePassword(Users[0].Username,Passwords[2],Passwords[2]));
        

        [Test]
        public async Task ChangePasswordTest() {

            //Change Password
            var U = await Agent.ChangePassword(Users[0].Username,Passwords[1],Passwords[0]);
            Assert.IsTrue(U.CheckPass(Passwords[1]),"Password did not get updated!");
            Assert.IsTrue((await Context.User.FindAsync(Users[0].Username) ?? new User()).CheckPass(Passwords[1]), "Did not save to DB!") ;
        }

        [Test]
        public void ResetPasswordFailTest() {

            //Sign in as a non-admin
            Guid SessionID = Manager.LogIn(Users[1].Username);

            //Attempt to change the password of another user
            Assert.ThrowsAsync<UserRolesException>(async () => await Agent.ResetPassword(SessionID, Users[2].Username, Passwords[4]));
        }

        [Test]
        public async Task ResetPasswordTest() {

            //Sign in as a admin
            Guid SessionID = Manager.LogIn(Users[0].Username);

            //Change and assert
            var U = await Agent.ResetPassword(SessionID, Users[1].Username, Passwords[2]);
            Assert.IsTrue(U.CheckPass(Passwords[2]), "Password did not get updated!");
            Assert.IsTrue((await Context.User.FindAsync(Users[1].Username) ?? new User()).CheckPass(Passwords[2]), "Did not save to DB!");
        }

        [Test]
        public void SetAdminFailTest() {
            //Sign in as a non-admin
            Guid SessionID = Manager.LogIn(Users[1].Username);

            Assert.ThrowsAsync<UserRolesException>(async () => await Agent.SetAdmin(SessionID, Users[0].Username, false), "Non-admin was able to unadmin an admin");
        }

        [Test]
        public void SetAdminSelfFailTest() {
            //Sign in as an admin
            Guid SessionID = Manager.LogIn(Users[0].Username);

            Assert.ThrowsAsync<SelfAdminException>(async () => await Agent.SetAdmin(SessionID, Users[0].Username, false), "admin was able to unadmin themselves");
        }

        [Test]
        public async Task SetAdminTest() {
            //Sign in as a admin
            Guid SessionID = Manager.LogIn(Users[0].Username);

            //Change and assert
            var U = await Agent.SetAdmin(SessionID, Users[1].Username, true);
            Assert.IsTrue(U.IsAdmin, "Admin did not get updated!");
            Assert.IsTrue((await Context.User.FindAsync(Users[1].Username) ?? new User()).IsAdmin, "Did not save to DB!");
        }

        [Test]
        public async Task SetImageTest() {
            //Sign in
            Guid SessionID = Manager.LogIn(Users[0].Username);

            const string ImageURL = "http://www.google.com";

            //Change and assert
            var U = await Agent.SetImage(SessionID, ImageURL);
            Assert.IsTrue(U.IsAdmin, "Admin did not get updated!");
            Assert.AreEqual(ImageURL, (await Context.User.FindAsync(Users[0].Username) ?? new User()).ImageURL, "Did not save to DB!");
        }

        [Test]
        public void LoginFailTest() => 
            _ = Assert.ThrowsAsync<PasswordIncorrectException>(async () => await Agent.LogIn(Users[0].Username, Passwords[1]), "User was able to sign in with wrong password");

        [Test]
        public async Task LoginTest() {
            Guid SessionID = await Agent.LogIn(Users[0].Username, Passwords[0]);
            Assert.AreEqual(Users[0].Username, (Manager.FindSession(SessionID) ?? new Session("")).Username,"User did not actually log in!");
        }

        [Test]
        public async Task LogoutTest() {
            //Sign in
            Guid SessionID = Manager.LogIn(Users[0].Username);

            //Logout through agent
            await Agent.LogOut(SessionID);

            //Assert we cannot find it in the manager anymore
            Assert.IsNull(Manager.FindSession(SessionID));
        }

        [Test]
        public async Task LogOutAllTest() {

            Assert.AreEqual(0, Manager.Count, "Manager was not reset");

            //Sign In a few times
            Guid SessionID = Manager.LogIn(Users[0].Username);
            _ = Manager.LogIn(Users[0].Username);
            _ = Manager.LogIn(Users[0].Username);
            _ = Manager.LogIn(Users[0].Username);

            Assert.AreEqual(4, Manager.Count, "Manager did not sign in properly");

            await Agent.LogOutAll(SessionID);

            Assert.AreEqual(0, Manager.Count, "Everyone did not sign out");

        }

        [Test]
        public void CreateUserFailTest() {
            Assert.ThrowsAsync<UsernameAlreadyExistsException>(async () 
                => await Agent.CreateUser(Users[0].Username, "Not Chopo", "lalaala"), "A Second Chopo has been added");
        }

        [Test]
        public async Task CreateUserTest() {
            await Agent.CreateUser("Chaka", "La Chaka", "123chachacha");
            Assert.IsNotNull(await Context.User.FindAsync("Chaka"), "Was not saved to DB!");
        }

        [Test]
        public async Task CreateFirstAdminTest() {
            Context.User.RemoveRange(Context.User); //Clear the DB
            await Context.SaveChangesAsync();

            var U = await Agent.CreateUser("Chaka", "La Chaka", "123chachacha");
            Assert.IsTrue(U.IsAdmin);
            Assert.IsNotNull(await Context.User.FindAsync("Chaka"), "Was not saved to DB!");
        }
    }
}