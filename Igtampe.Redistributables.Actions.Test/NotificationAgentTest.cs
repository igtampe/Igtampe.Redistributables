using Igtampe.Actions;
using Igtampe.ChopoAuth;
using Igtampe.ChopoSessionManager;
using Igtampe.Notifier;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Igtampe.Redistributables.Actions.Test {
    public class NotificationActionAgentTests {

        private static readonly User[] Users = {
            new() { Username = "Chopo", IsAdmin = true, Name = "Chopo" },
            new() { Username = "Chapo", Name = "Chapo" },
        };

        private static readonly Notification[] Notifications = {
            new() { ID =Guid.NewGuid(), Text="Hoo hoo ha ha heee hee" },
            new() { ID =Guid.NewGuid(), Text="Christmas is Here" },
            new() { ID =Guid.NewGuid(), Text="Sometimes people die and that's a shame" },
            new() { ID =Guid.NewGuid(), Text="Yarg, you have scurvy" },
            new() { ID =Guid.NewGuid(), Text="B" },
        };

        private readonly ISessionManager Manager = SessionManager.Manager;
        private readonly NotificationAgent<TestContext,Notification<User>,User> Agent;
        private TestContext Context;

        public NotificationActionAgentTests() {

            Notifications[0].Owner = Users[0];
            Notifications[1].Owner = Users[0];
            Notifications[2].Owner = Users[1];
            Notifications[3].Owner = Users[1];
            Notifications[4].Owner = Users[1];

            Context = SetupContext();
            Agent = new(Context, Manager);
        }

        private TestContext SetupContext() {
            var Factory = new TestContextFactory(nameof(NotificationActionAgentTests));
            Context = Factory.Create();

            FillContext();
            return Context;
        }

        private void FillContext() {
            Context.AddRange(Notifications);
            Context.SaveChanges();
        }

        private void EmptyContext() {
            Context.RemoveRange(Context.Notification);
            Context.SaveChanges();
        }

        private void ResetContext() {
            Manager.Reset();
            EmptyContext();
            FillContext();
        }

        [SetUp]
        public void Setup() => ResetContext();

        [Test]
        public async Task GetAllTest() {
            var S1 = Manager.LogIn(Users[0].Username);
            var S2 = Manager.LogIn(Users[1].Username);

            var U1Ns = await Agent.GetAll(S1);
            var U2Ns = await Agent.GetAll(S2);

            Assert.AreEqual(2, U1Ns.Count);
            Assert.AreEqual(3, U2Ns.Count);
        }

        [Test]
        public async Task DeleteOneFailTest() {
            var S1 = Manager.LogIn(Users[0].Username);
            
            Assert.AreEqual(5, Context.Notification.Count(),"Notification Count is incorrect");

            await Agent.DeleteOne(S1, Notifications[3].ID);

            Assert.AreEqual(5, Context.Notification.Count(), "Notification was deleted (?)");
        }

        [Test]
        public async Task DeleteOneTest() {
            var S1 = Manager.LogIn(Users[0].Username);

            Assert.AreEqual(5, Context.Notification.Count(), "Notification Count is incorrect");

            await Agent.DeleteOne(S1, Notifications[0].ID);

            Assert.AreEqual(4, Context.Notification.Count(), "Notification wasn't deleted");
            Assert.IsNull(await Context.Notification.FindAsync(Notifications[0].ID),"Notification wasn't deleted");
        }

        [Test]
        public async Task DeleteAllTest() {
            var S1 = Manager.LogIn(Users[1].Username);

            Assert.AreEqual(5, Context.Notification.Count(), "Notification Count is incorrect");

            await Agent.DeleteAll(S1);

            Assert.AreEqual(2, Context.Notification.Count(), "Notification wasn't deleted");
            Assert.IsFalse(await Context.Notification.AnyAsync(A => A.Owner!=null && A.Owner.Username == Users[1].Username));
        }
    }
}