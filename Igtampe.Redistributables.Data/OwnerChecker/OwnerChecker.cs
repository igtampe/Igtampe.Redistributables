using static Igtampe.Data.SqlBuilder.SqlBuilder;

namespace Igtampe.Data.OwnerChecker {

    /// <summary>Utility class to verify ownership of an object</summary>
    public static class OwnerChecker {

        /// <summary>Checks if a user owns this object</summary>
        /// <param name="template">Currently used adoTemplate</param>
        /// <param name="table">Table to check in</param>
        /// <param name="username">Username to check</param>
        /// <param name="idColumn">ID Column of the table</param>
        /// <param name="id">ID of the object to check the user owns</param>
        /// <param name="usernameColumn">Username column on this table that indicates ownership</param>
        /// <returns>True if the username column's value for this id row matches the expected username (IE: The user owns this object)</returns>
        public static async Task<bool> UserOwns(AdoTemplate.AdoTemplate template, string table, string username, string idColumn, int id, string usernameColumn = "USER_NM") {
            var sql = Select(["COUNT(*)"], table).Where(new WhereConditionGroup([new(usernameColumn), new(idColumn)])).ToString();
            return await template.QuerySingle(sql, (cmd) => {
                cmd.SetString(usernameColumn, username);
                cmd.SetInt(idColumn, id);
            }, (reader) => reader.GetInt(0) == 1);
        }


        /// <summary>Checks if a user owns all of this object</summary>
        /// <param name="template">Currently used adoTemplate</param>
        /// <param name="table">Table to check in</param>
        /// <param name="username">Username to check</param>
        /// <param name="idColumn">ID column of the table</param>
        /// <param name="ids">IDs of the objects to check the user owns</param>
        /// <param name="usernameColumn">Username column on this table that indicates ownership</param>
        /// <returns>True if the username column's value for all rows with given IDs matches expected username (IE: User owns all objects)</returns>
        public static async Task<bool> UserOwnsAll(AdoTemplate.AdoTemplate template, string table, string username, string idColumn, List<int> ids, string usernameColumn = "USER_NM") {
            var sql = Select(["COUNT(*)"], table).Where(new WhereConditionGroup([new(usernameColumn), new(idColumn, ids.Select(A => $"{A}").ToList())])).ToString();
            return await template.QuerySingle(sql, (cmd) => cmd.SetString(usernameColumn, username), (reader) => reader.GetInt(0) == ids.Count);
        }
    }
}
