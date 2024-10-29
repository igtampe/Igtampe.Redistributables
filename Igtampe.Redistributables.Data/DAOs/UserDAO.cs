using Igtampe.ChopoAuth;
using static Igtampe.Data.AdoTemplate.AdoTemplate;
using static Igtampe.Data.SqlBuilder.SqlBuilder;

namespace Igtampe.Data.DAOs {

    /// <summary>Base DAO for any kind of user</summary>
    /// <typeparam name="T">Type of your user</typeparam>
    /// <param name="connectionString">Connection string to DB</param>
    /// <param name="table">User table</param>
    /// <param name="usernameColumn">Username column</param>
    /// <param name="passwordColumn">Password column</param>
    public abstract class UserDAO<T>(string connectionString, string table, string usernameColumn = "USER_NM", string passwordColumn ="PASS_TX"){
        readonly Hashbrown.Hashbrown hashbrown = new();
        readonly AdoTemplate.AdoTemplate adoTemplate = new(connectionString);

        /// <summary>Gets the full object for this user</summary>
        /// <param name="username">Username to get</param>
        /// <returns>A user</returns>
        public abstract Task<T?> GetUser(string username); 

        /// <summary>Authenticates a given user</summary>
        /// <param name="username">Username to check</param>
        /// <param name="password">Password to check</param>
        /// <returns>True if the username and password match</returns>
        public async Task<bool> Authenticate(string username, string password) {

            var sql = $"SELECT COUNT(*) FROM {table} WHERE {usernameColumn} = @username AND {passwordColumn} = @password";

            return await adoTemplate.QuerySingle(sql,
                (cmd) => {
                    cmd.SetString("username", username);
                    cmd.SetString("password", hashbrown.Hash(password));
                },
                (reader) => reader.GetInt(0) > 0);

        }

        /// <summary>Registers a user</summary>
        /// <param name="username">Username of the user (must be unique)</param>
        /// <param name="password">Password of the user</param>
        /// <param name="additionalColumns">List of additional columns to set when registering this user</param>
        /// <param name="setter">Setter for the additional columns for this user</param>
        /// <returns></returns>
        protected virtual  async Task Register(string username, string password, List<string> additionalColumns, Action<Setter> setter) {

            List<string> columns = [usernameColumn, passwordColumn];
            columns.AddRange(additionalColumns);

            var sql = Insert(table, columns).ToString();

            await adoTemplate.Execute(sql, (cmd) => {
                cmd.SetString(usernameColumn, username);
                cmd.SetString(passwordColumn, hashbrown.Hash(password));
                setter(cmd);
            });
        }

        /// <summary>Updates the password of the given user if the oldpassword matches the current password</summary>
        /// <param name="username">Username to update</param>
        /// <param name="oldPassword">Old (or current) password</param>
        /// <param name="newPassword">New passowrd</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown if the old (or current) password is incorrect</exception>
        public async Task UpdatePassword(string username, string oldPassword, string newPassword) {

            if (!await Authenticate(username, oldPassword)) {
                throw new ArgumentException("Incorrect password");
            }

            var sql = Update(table, [passwordColumn])
                .Where(new WhereConditionGroup([new(usernameColumn)])).ToString();
            
            await adoTemplate.Execute(sql, (cmd) => {
                cmd.SetString(usernameColumn, username);
                cmd.SetString(passwordColumn, hashbrown.Hash(newPassword));
            });
        }
    }
}

