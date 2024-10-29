using static Igtampe.Data.SqlBuilder.SqlBuilder;

namespace Igtampe.Data.DAOs {

    /// <summary>Basic DAO for Chopo Sessions</summary>
    /// <param name="sessionLifetimeHours">Timespan in hours for a session to be considered live. Sessions which have been untouched for this number of hours will be considered expired </param>
    /// <param name="connectionString">Connection string to the DB</param>
    /// <param name="table">Table where the sessions are stored</param>
    /// <param name="sessionColumn">Column which holds the session ID as a string</param>
    /// <param name="usernameColumn">Column which holds the session username as a string</param>
    /// <param name="touchColumn">Column which holds the last touch time of the session as Timestamp</param>
    public class SessionDAO(string table, string connectionString, int sessionLifetimeHours = 1, string sessionColumn = "SESSION_ID", string usernameColumn = "USER_NM", string touchColumn = "TOUCH_TS" ) {

        readonly AdoTemplate.AdoTemplate adoTemplate = new(connectionString);
        readonly Hashbrown.Hashbrown hashbrown = new("SESSION_SALT");

        /// <summary>Adds a session for the given user</summary>
        /// <param name="username"></param>
        /// <returns>GUID of this session</returns>
        public async Task<Guid> AddSession(string username) {
            Guid guid = new();

            await adoTemplate.Execute(Insert(table, [sessionColumn, usernameColumn, touchColumn])
                .SetValues(new Dictionary<string, string>() {{ touchColumn, "CURRENT_TIMESTAMP" }} )
                .ToString(),
                (setter) => {
                    setter.SetString(sessionColumn, HashGuid(guid));
                    setter.SetString(usernameColumn, username);
                });

            return guid;

        }

        /// <summary>Retrieves a session's associated username, and extends it</summary>
        /// <param name="id">ID of the session to retrieve</param>
        /// <returns>Username of the session</returns>
        public async Task<string?> GetSession(Guid id) {
            return await adoTemplate.QuerySingle(
                    Update(table, [touchColumn])
                    .SetValues(new Dictionary<string, string>() { { touchColumn, "CURRENT_TIMESTAMP" } })
                    .Where(new WhereConditionGroup([
                            new(sessionColumn),
                            new(touchColumn,WhereConditionOperator.GREATER_THAN,$"NOW() - INTERVAL '{sessionLifetimeHours} hours'")
                        ]))
                    .Returning([usernameColumn])
                .ToString(),
                (setter) => setter.SetString(sessionColumn, HashGuid(id)),
                (getter) => getter.GetString(0)
            );
        }

        /// <summary>Removes session with given ID</summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task RemoveSession(Guid id) {
            await adoTemplate.Execute(
                Delete(table)
                .Where(new WhereCondition(sessionColumn)).ToString(),
                (setter) => setter.SetString(sessionColumn, HashGuid(id))
            );
        }

        /// <summary>Removes all session with the specified username</summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task RemoveAllSessions(string username) {
            await adoTemplate.Execute(
                Delete(table)
                .Where(new WhereCondition(usernameColumn)).ToString(),
                (setter) => setter.SetString(usernameColumn, username)
            );
        }

        /// <summary>Removes all expired sessions</summary>
        /// <returns></returns>
        public async Task PruneSessions() {
            await adoTemplate.Execute(
                Delete(table)
                .Where(new WhereCondition(touchColumn, WhereConditionOperator.LESS_THAN, $"NOW() - INTERVAL '{sessionLifetimeHours} hours'")).ToString()
            );
        }

        private string HashGuid(Guid guid) {
            return hashbrown.Hash(guid.ToString());
        }

    }
}
