using Igtampe.Actions;
using Igtampe.ChopoAuth.Exceptions;
using Igtampe.ChopoImageHandling;
using Igtampe.ChopoSessionManager;
using Igtampe.Toffee.Common;
using Igtampe.Toffee.Data;

namespace Igtampe.Toffee.Actions {

    /// <summary>Agent to act upon all Toffee Users</summary>
    public class UserAgent : AuthAgent<ToffeeContext,User> {

        /// <summary>Creates a User Agent</summary>
        /// <param name="Context"></param>
        /// <param name="Manager"></param>
        public UserAgent(ToffeeContext Context, ISessionManager Manager) : base(Context, Manager) {}

        /// <summary>Updates a User's Rank</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="Username">Username to update the rank of</param>
        /// <param name="NewRank">New rank for the user</param>
        /// <returns></returns>
        /// <exception cref="UserRolesException">Thrown if Session executing is not an admin</exception>
        public async Task<User> UpdateUserRank(Guid? SessionID, string Username, int NewRank) {
            if (!await SessionIsAdmin(SessionID)) { throw new UserRolesException("", "Admin"); }
            User U = await GetUser(Username);
            U.Rank = NewRank;
            await SaveUser(U);
            return U;
        }

        /// <summary>Updates a User's profile image</summary>
        /// <param name="SessionID"></param>
        /// <param name="Profile"></param>
        /// <returns></returns>
        public async Task<User> UpdateUserProfile(Guid? SessionID, Image Profile) {
            var U = await GetMe(SessionID);

            Image? OldProfile = U.ProfilePicture;

            U.ProfilePicture = Profile;
            await SaveUser(U);
            if (OldProfile is not null) { Context.Image.Remove(Profile); }

            return U;
        }
    }
}
