using Igtampe.Actions.Exceptions;
using Igtampe.ChopoAuth;
using Igtampe.ChopoAuth.Exceptions;
using Igtampe.ChopoSessionManager;
using Igtampe.ChopoSessionManager.Exceptions;
using Igtampe.DBContexts;
using Microsoft.EntityFrameworkCore;

namespace Igtampe.Actions {

    /// <summary>ActionAgent dealing with UserAuthentication in the Igtampe Redistributables Package (With the default <see cref="User"/> implementation)</summary>
    /// <typeparam name="E">DBContext that is also a <see cref="IUserContext{User}"/></typeparam>
    public class AuthAgent<E> : AuthAgent<E, User> where E:DbContext,IUserContext<User>{
        /// <summary>Creates an AuthAgent</summary>
        /// <param name="Context">Context to operate on</param>
        /// <param name="Manager">SessionManager that tracks who is signed in or not</param>
        public AuthAgent(E Context, ISessionManager Manager) : base(Context, Manager) {}
    }

    /// <summary>ActionAgent dealing with User Authentication in the Igtampe Redistributables package</summary>
    /// <typeparam name="E">DBContext that is also a <see cref="IUserContext{F}"/></typeparam>
    /// <typeparam name="F">Type of User based on <see cref="User"/></typeparam>
    public class AuthAgent<E, F> : SessionedActionAgent<E> where E : DbContext, IUserContext<F> where F : User, new() {

        /// <summary>Creates an AuthAgent</summary>
        /// <param name="Context">Context to operate on</param>
        /// <param name="Manager">SessionManager that tracks who is signed in or not</param>
        public AuthAgent(E Context, ISessionManager Manager) : base(Context, Manager) { }

        /// <summary>Gets the directory of all the users in the system</summary>
        /// <param name="Query">Search query to look for in Username and Name</param>
        /// <param name="Take">Amount of users to take from the list</param>
        /// <param name="Skip">Amount of users to skip over when taking from the list</param>
        /// <returns>A list of users that match the criteria. If none were found, returns an empty list</returns>
        public async Task<List<F>> GetDirectory(string? Query = "", int? Take = null, int? Skip = null) {
            IQueryable<F> Set = Context.User; //Get the users
            if (!string.IsNullOrWhiteSpace(Query)) { //If we have a query
                Query = Query.Trim().ToLower(); //Clean it up and lower it because we compare lowered
                Set = Set.Where(U =>  //Look for matches where the username or name contains
                    (U.Username != null && U.Username.ToLower().Contains(Query) ||
                    (U.Name != null && U.Name.ToLower().Contains(Query))
                ));
            }

            Set = Set.Skip(Skip ?? 0).Take(Take ?? 20); //Apply skip and take

            return await Set.ToListAsync(); //Return adios
        }

        /// <summary>Gets the user object tied to a given signed in session's id</summary>
        /// <param name="SessionID">ID of the session</param>
        /// <returns>The user object of the user tied to the session</returns>
        /// <exception cref="SessionNotFoundException">Thrown if the session was not found</exception>
        public async Task<F> GetMe(Guid? SessionID)
            => await GetUser((await GetSession(SessionID)).Username);

        /// <summary></summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        /// <exception cref="UserNotFoundException"></exception>
        public async Task<F> GetUser(string Username) {
            F? U = await Context.ApplyAutoIncludes(Context.User).FirstOrDefaultAsync(U => U.Username == Username); //Get the user (with autoincludes)
            return U is null ? throw new UserNotFoundException(Username) : U; //Throw if we didn't find, else get the user
        }

        /// <summary>Checks if a User is an Admin</summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public async Task<bool> UserIsAdmin(string Username) =>
            (await GetUser(Username)).IsAdmin;

        /// <summary>Checks if a session is admin</summary>
        /// <param name="SessionID"></param>
        /// <returns></returns>
        public async Task<bool> SessionIsAdmin(Guid? SessionID) =>
            await UserIsAdmin((await GetSession(SessionID)).Username);

        /// <summary>Changes the password of a signed in user</summary>
        /// <param name="SessionID">ID of the session of the signed in user</param>
        /// <param name="New">New password</param>
        /// <param name="Current">Current Password</param>
        /// <returns></returns>
        /// <exception cref="SessionNotFoundException">Thrown if session wasn't found</exception>
        /// <exception cref="PasswordIncorrectException">Thrown if current password was incorrect</exception>
        public async Task<F> ChangePassword(Guid? SessionID, string New, string Current) =>
            await ChangePassword((await GetSession(SessionID)).Username, New, Current);

        /// <summary>Changes the password of a given user</summary>
        /// <param name="Username">User to change the password of</param>
        /// <param name="New">New password</param>
        /// <param name="Current">Current Password</param>
        /// <returns></returns>
        /// <exception cref="PasswordIncorrectException">If the current password isn't correct</exception>
        public async Task<F> ChangePassword(string Username, string New, string Current) {
            //Check the password
            var U = await GetUser(Username);
            if (!U.CheckPass(Current)) { throw new PasswordIncorrectException(Username); }

            U.UpdatePass(New);
            return await SaveUser(U);
        }

        /// <summary>Resets a password for another user</summary>
        /// <param name="SessionID">Session executing the reset password request</param>
        /// <param name="Username">User to reset the password of</param>
        /// <param name="New">New password</param>
        /// <returns></returns>
        /// <exception cref="UserRolesException">Thrown if Session executing isn't an admin</exception>
        public async Task<F> ResetPassword(Guid? SessionID, string Username, string New) {
            if(!(await SessionIsAdmin(SessionID))) { throw new UserRolesException("", "Admin"); }

            var U = await GetUser(Username);
            U.UpdatePass(New);

            return await SaveUser(U);

        }

        /// <summary>Sets admin of a user</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="User">User to change admin status</param>
        /// <param name="Admin">Status to update admin to</param>
        /// <returns></returns>
        /// <exception cref="UserRolesException">Thrown if session executing the request is not an admin</exception>
        /// <exception cref="SelfAdminException">Thrown if session executing the request is tied to the user being modified</exception>
        public async Task<F> SetAdmin(Guid? SessionID, string User, bool Admin) {
            var Requester = await GetMe(SessionID);

            if (!Requester.IsAdmin) { throw new UserRolesException(Requester.Username, "Admin"); }
            if (Requester.Username == User) { throw new SelfAdminException(User); }

            var U = await GetUser(User);
            U.IsAdmin = Admin;

            return await SaveUser(U);
        }

        /// <summary>Modifies Session User's ImageURL field</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="ImageURL">URL to the new profile image for this user</param>
        /// <returns></returns>
        public async Task<F> SetImage(Guid? SessionID, string ImageURL) {
            var U = await GetMe(SessionID);
            U.ImageURL = ImageURL;
            return await SaveUser(U);
        }

        /// <summary>Logs a User In</summary>
        /// <param name="Username">Username of the user</param>
        /// <param name="Password">Password attempt of the user</param>
        /// <returns>A Session ID for the user who logged in</returns>
        /// <exception cref="PasswordIncorrectException">Thrown if password is incorrect</exception>
        public async Task<Guid> LogIn(string Username, string Password) {
            try {
                return !(await GetUser(Username)).CheckPass(Password)
                    ? throw new PasswordIncorrectException(Username)
                    : Manager.LogIn(Username);
            } catch (UserNotFoundException) { throw new PasswordIncorrectException(Username); } //Obfuscates UserNotFound
        }

        /// <summary>Delegates to the Session Manager to logout</summary>
        /// <param name="SessionID"></param>
        /// <returns>True or false depending on if log out was successful</returns>
        public async Task<bool> LogOut(Guid SessionID) 
            => await Task.Run(() => Manager.LogOut(SessionID));

        /// <summary>Delegates to SessionManager to log out of all sessions tied to the same user the given session is</summary>
        /// <param name="SessionID"></param>
        /// <returns>Number of sessions logged out of</returns>
        public async Task<int> LogOutAll(Guid SessionID) 
            => await Task.Run(async () => Manager.LogOutAll((await GetSession(SessionID)).Username));

        /// <summary>Creates a User in the DB</summary>
        /// <param name="Username">Username of the user</param>
        /// <param name="Name">Real name of the user</param>
        /// <param name="Password">Password of the user</param>
        /// <returns>The user</returns>
        /// <exception cref="UsernameAlreadyExistsException">Thrown if the user already exists</exception>
        public async Task<F> CreateUser(string Username, string Name, string Password) {
            F NewUser = new() { Username = Username, Name = Name };
            NewUser.UpdatePass(Password);

            if (!await Context.User.AnyAsync()) {
                //This is the first account and *MUST* be an admin
                NewUser.IsAdmin = true;
            } else if (Context.User.Any(U => U.Username == Username)) {
                //This check doesn't need to run if there isn't any users so we can put it as an else if
                throw new UsernameAlreadyExistsException(Username);
            }

            Context.User.Add(NewUser);
            await Context.SaveChangesAsync();

            return NewUser;
        }
        
        /// <summary>Saves a user to the database</summary>
        /// <param name="U"></param>
        /// <returns></returns>
        private async Task<F> SaveUser(F U) {
            Context.User.Update(U);
            await Context.SaveChangesAsync();
            return U;
        }
    }
}
