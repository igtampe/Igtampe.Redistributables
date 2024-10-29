namespace Igtampe.Data.DAOs {

    /// <summary>A UserDAO that expects a register key in order to register</summary>
    /// <typeparam name="T">Type of your user</typeparam>
    /// <param name="connectionString">Connection string to DB</param>
    /// <param name="table">User table</param>
    /// <param name="usernameColumn">Username column</param>
    /// <param name="passwordColumn">Password column</param>
    /// <param name="registrationKey">Key to expect when registering</param>
    public abstract class RegistrationKeyUserDAO<T>(string connectionString, string table, string? registrationKey, string usernameColumn = "USER_NM", string passwordColumn = "PASS_TX") 
        : UserDAO<T>(connectionString, table, usernameColumn, passwordColumn) {

        /// <summary>Registers a user, validating if their inputted key is correct</summary>
        /// <param name="username">Username of the user (must be unique)</param>
        /// <param name="password">Password of the user</param>
        /// <param name="additionalColumns">List of additional columns to set when registering this user</param>
        /// <param name="setter">Setter for the additional columns for this user</param>
        /// <param name="key">Registration key the user has inputted</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">If the provided key is incorrect, or if no registration key is currently set</exception>
        protected async Task Register(string username, string password, string key, List<string> additionalColumns, Action<AdoTemplate.AdoTemplate.Setter> setter) {

            if ((registrationKey?.ToString().Length ?? 0) == 0) {
                throw new ArgumentException("No Registrations are accepted at this time");
            }

            if (!key.Equals(registrationKey?.ToString())) {
                throw new ArgumentException("Registration key is incorrect");
            }

            await base.Register(username, password, additionalColumns, setter);
        }

    }
}
