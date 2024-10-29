using Igtampe.ChopoSessionManager;
using Igtampe.Data.DAOs;

namespace Igtampe.Data.DBSessionManager {

    /// <summary>This class is provided for drop in compatibility with existing users of the in memory session manager</summary>
    /// <param name="table"></param>
    /// <param name="connectionString"></param>
    /// <param name="sessionLifetimeHours"></param>
    /// <param name="sessionColumn"></param>
    /// <param name="usernameColumn"></param>
    /// <param name="touchColumn"></param>
    public class DbSessionManager(string table, string connectionString, int sessionLifetimeHours = 1, string sessionColumn = "SESSION_ID", string usernameColumn = "USER_NM", string touchColumn = "TOUCH_TS") : ISessionManager {

        readonly SessionDAO dao = new(table,connectionString, sessionLifetimeHours, sessionColumn, usernameColumn, touchColumn);

        /// <summary>Gets a count of all sessions</summary>
        public int Count => throw new NotImplementedException();


        /// <summary>Extends a given session</summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool ExtendSession(Guid ID) {
            var t = dao.GetSession(ID);
            t.Wait();
            return t.Result != null;
        }

        /// <summary>Finds a given session</summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Session? FindSession(Guid? ID) {
            if (ID == null) return null;
            var t = dao.GetSession(ID ?? Guid.Empty);
            t.Wait();
            return t.Result==null? null : new Session(t.Result);
        }

        /// <summary>Logs in a user</summary>
        /// <param name="UserID"></param>
        /// <returns>Session ID for this user's session</returns>
        public Guid LogIn(string UserID) {
            var t = dao.AddSession(UserID);
            t.Wait();
            return t.Result;
        }

        /// <summary>Logs out a user</summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool LogOut(Guid ID) {
            var t = dao.RemoveSession(ID);
            t.Wait();
            return true; //Just always return true;
        }


        /// <summary>Logs out a user from all their sessions</summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public int LogOutAll(string Username) {
            var t = dao.RemoveAllSessions(Username);
            t.Wait();
            return 1; //Just always return 1;
        }

        /// <summary>Removes sessions that are currently expired</summary>
        /// <returns></returns>
        public int RemoveExpiredSessions() {
            var t = dao.PruneSessions();
            t.Wait();
            return 1;
        }

        /// <summary>Resets the session manager</summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">This method is not implemented</exception>
        public int Reset() => throw new NotImplementedException();
        
    }
}
