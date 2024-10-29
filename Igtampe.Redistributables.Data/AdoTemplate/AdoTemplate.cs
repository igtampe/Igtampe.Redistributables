using Npgsql;
using NpgsqlTypes;
using System.Data;

namespace Igtampe.Data.AdoTemplate {

    /// <summary>ADOTempalte for PostgresSQL compatible DBs. Similar to JDBCTemplate</summary>
    /// <param name="connectionString">Connection string to your DB in ADO.NET format</param>
    public class AdoTemplate(string connectionString) {

        /// <summary>Setter object holding functions to set items in your parametrized query</summary>
        /// <param name="InternalSet"></param>
        public class Setter(Action<string, NpgsqlDbType, object?> InternalSet) {

            /// <summary>Alias to Set(key,NpgsqlDbType.Boolean,value)</summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            public void SetBoolean(string key, bool? value) => InternalSet(key, NpgsqlDbType.Boolean, value);

            /// <summary>Alias to Set(key,NpgsqlDbType.Integer,value)</summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            public void SetInt(string key, int? value) => InternalSet(key, NpgsqlDbType.Integer, value);

            /// <summary>Alias to Set(key,NpgsqlDbType.Bigint,value)</summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            public void SetLong(string key, long? value) => InternalSet(key, NpgsqlDbType.Bigint, value);

            /// <summary>Alias to Set(key,NpgsqlDbType.Double,value)</summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            public void SetDouble(string key, double? value) => InternalSet(key, NpgsqlDbType.Double, value);

            /// <summary>Alias to Set(key,NpgsqlDbType.Varchar,value)</summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            public void SetString(string key, string? value) => InternalSet(key, NpgsqlDbType.Varchar, value);

            /// <summary>Alias to Set(key,NpgsqlDbType.Uuid,value)</summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            public void SetGuid(string key, Guid? value) => InternalSet(key, NpgsqlDbType.Uuid, value);

            /// <summary>Alias to Set(key,NpgsqlDbType.Bytea,value)</summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            public void SetBytea(string key, byte[]? value) => InternalSet(key, NpgsqlDbType.Bytea, value);

            /// <summary>Alias to Set(key,NpgsqlDbType.TimestampTz,value)</summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            public void SetTimestamp(string key, DateTime? value) => InternalSet(key, NpgsqlDbType.TimestampTz, value);

            /// <summary>Alias to Set(key,NpgsqlDbType.Date,value)</summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            public void SetDate(string key, DateTime? value) => InternalSet(key, NpgsqlDbType.Date, value);

            /// <summary>External facing set</summary>
            /// <param name="key"></param>
            /// <param name="type"></param>
            /// <param name="value"></param>
            public void Set(string key, NpgsqlDbType type, object value) => InternalSet(key, type, value);

        }

        /// <summary>Getter to retrieve specific values from a DB result</summary>
        /// <param name="reader"></param>
        public class Getter(IDataReader reader) {

            /// <summary>Checks if the result contains the given key</summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public bool ContainsKey(string key) {
                try { reader.GetOrdinal(key); }
                catch (IndexOutOfRangeException) { return false; }
                return true;
            }

            /// <summary>Checks whether the result for this ket is null</summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public bool IsNull(string key) => !ContainsKey(key) || reader.IsDBNull(reader.GetOrdinal(key));

            /// <summary>Gets Boolean under this column name</summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public bool GetBoolean(string key) => GetBoolean(reader.GetOrdinal(key));

            /// <summary>Gets Integer under this column name</summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public int GetInt(string key) => GetInt(reader.GetOrdinal(key));

            /// <summary>Gets long under this column name</summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public long GetLong(string key) => GetLong(reader.GetOrdinal(key));

            /// <summary>Gets Double under this column name</summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public double GetDouble(string key) => GetDouble(reader.GetOrdinal(key));

            /// <summary>Gets String under this column name</summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public string GetString(string key) => GetString(reader.GetOrdinal(key));

            /// <summary>Gets DateTime under this column name</summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public DateTime GetDateTime(string key) => GetDateTime(reader.GetOrdinal(key));

            /// <summary>Gets Guid under this column name</summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public Guid GetGuid(string key) => GetGuid(reader.GetOrdinal(key));

            /// <summary>Gets Bytes under this column name</summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public byte[] GetBytea(string key) => (byte[])reader[key];


            /// <summary>Gets Boolean at this column index</summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public bool GetBoolean(int index) => reader.GetBoolean(index);

            /// <summary>Gets Int at this column index</summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public int GetInt(int index) => reader.GetInt32(index);

            /// <summary>Gets Long at this column index</summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public long GetLong(int index) => reader.GetInt64(index);

            /// <summary>Gets Double at this column index</summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public double GetDouble(int index) => reader.GetDouble(index);

            /// <summary>Gets String at this column index</summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public string GetString(int index) => reader.GetString(index);

            /// <summary>Gets DateTime at this column index</summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public DateTime GetDateTime(int index) => reader.GetDateTime(index);

            /// <summary>Gets Guid at this column index</summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public Guid GetGuid(int index) => reader.GetGuid(index);


            /// <summary>Gets optional Boolean at this column name</summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public bool? GetOptionalBoolean(string key) => GetOptionalBoolean(reader.GetOrdinal(key));

            /// <summary>Gets optional Int at this column name</summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public int? GetOptionalInt(string key) => GetOptionalInt(reader.GetOrdinal(key));

            /// <summary>Gets optional Long at this column name</summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public long? GetOptionalLong(string key) => GetOptionalLong(reader.GetOrdinal(key));

            /// <summary>Gets optional Double at this column name</summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public double? GetOptionalDouble(string key) => GetOptionalDouble(reader.GetOrdinal(key));

            /// <summary>Gets optional Stirng at this column name</summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public string? GetOptionalString(string key) => GetOptionalString(reader.GetOrdinal(key));

            /// <summary>Gets optional DateTime at this column name</summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public DateTime? GetOptionalDateTime(string key) => GetOptionalDateTime(reader.GetOrdinal(key));

            /// <summary>Gets optional Guid at this column name</summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public Guid? GetOptionalGuid(string key) => GetOptionalGuid(reader.GetOrdinal(key));

            /// <summary>Gets optional Byte Array at this column name</summary>
            /// <param name="key"></param>
            /// <returns></returns>
            public byte[]? GetOptionalBytea(string key) => reader.IsDBNull(reader.GetOrdinal(key)) ? null : (byte[])reader[key];

            /// <summary>Gets optional Boolean at this column index</summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public bool? GetOptionalBoolean(int index) => reader.IsDBNull(index) ? null : reader.GetBoolean(index);

            /// <summary>Gets optional Int at this column index</summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public int? GetOptionalInt(int index) => reader.IsDBNull(index) ? null : reader.GetInt32(index);

            /// <summary>Gets optional Long at this column index</summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public long? GetOptionalLong(int index) => reader.IsDBNull(index) ? null : reader.GetInt64(index);

            /// <summary>Gets optional Double at this column index</summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public double? GetOptionalDouble(int index) => reader.IsDBNull(index) ? null : reader.GetDouble(index);

            /// <summary>Gets optional String at this column index</summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public string? GetOptionalString(int index) => reader.IsDBNull(index) ? null : reader.GetString(index);

            /// <summary>Gets optional DateTime at this column index</summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public DateTime? GetOptionalDateTime(int index) => reader.IsDBNull(index) ? null : reader.GetDateTime(index);

            /// <summary>Gets optional Guid at this column index</summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public Guid? GetOptionalGuid(int index) => reader.IsDBNull(index) ? null : reader.GetGuid(index);

        }


        private readonly string ConnectionString = connectionString;

        /// <summary>Runs a Test query to see if the DB is connected</summary>
        /// <returns></returns>
        public async Task Test() {
            await Execute("Select 1");
        }

        /// <summary>Conducts a query for a single result from DB</summary>
        /// <typeparam name="T">Type of expected return</typeparam>
        /// <param name="sql">Base SQL for this call with parameters</param>
        /// <param name="setter">Setter for this call to set parameters</param>
        /// <param name="rowMapper">Mapper using Getter to map return row to object</param>
        /// <returns>The first or default from the results as mapped by provided rowmapper</returns>
        public async Task<T?> QuerySingle<T>(string sql, Action<Setter> setter, Func<Getter, T> rowMapper) {
            return (await Query(sql, setter, rowMapper)).FirstOrDefault();
        }

        /// <summary>Conducts a query for a single result from DB</summary>
        /// <typeparam name="T">Type of expected return</typeparam>
        /// <param name="sql">Base SQL for this call with parameters</param>
        /// <param name="setter">Setter for this call to set parameters</param>
        /// <param name="rowMapper">Mapper using Getter to map return row to object</param>
        /// <returns>The first or default from the results as mapped by provided rowmapper</returns>
        public async Task<T?> QuerySingle<T>(string sql, Action<Setter> setter, Func<Getter, Task<T>> rowMapper) {
            return (await Query(sql, setter, rowMapper)).FirstOrDefault();
        }

        /// <summary>Conducts a query for a list of results from DB</summary>
        /// <typeparam name="T">Type of expected return</typeparam>
        /// <param name="sql">Base SQL for this call with parameters</param>
        /// <param name="setter">Setter for this call to set parameters</param>
        /// <param name="rowMapper">Mapper using Getter to map return row to object</param>
        /// <returns>List of results from DB as mapped to type T by rowMapper</returns>
        public async Task<List<T>> Query<T>(string sql, Action<Setter> setter, Func<Getter, T> rowMapper) {
            var results = new List<T>();

            using var conn = new NpgsqlConnection(ConnectionString);
            await conn.OpenAsync();

            using var cmd = new NpgsqlCommand(sql, conn);

            void setParam(string key, NpgsqlDbType type, object? val) {
                cmd.Parameters.Add(new NpgsqlParameter(key, type) { Value = val ?? DBNull.Value });
            }

            setter(new Setter(setParam));

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync()) { results.Add(rowMapper(new Getter(reader))); }

            return results;

        }

        /// <summary>Conducts a query for a list of results from DB</summary>
        /// <typeparam name="T">Type of expected return</typeparam>
        /// <param name="sql">Base SQL for this call with parameters</param>
        /// <param name="setter">Setter for this call to set parameters</param>
        /// <param name="rowMapper">Mapper using Getter to map return row to object</param>
        /// <returns>List of results from DB as mapped to type T by rowMapper</returns>
        public async Task<List<T>> Query<T>(string sql, Action<Setter> setter, Func<Getter, Task<T>> rowMapper) {
            var results = new List<T>();

            using var conn = new NpgsqlConnection(ConnectionString);
            await conn.OpenAsync();

            using var cmd = new NpgsqlCommand(sql, conn);

            void setParam(string key, NpgsqlDbType type, object? val) {
                cmd.Parameters.Add(new NpgsqlParameter(key, type) { Value = val ?? DBNull.Value });
            }

            setter(new Setter(setParam));

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync()) { results.Add(await rowMapper(new Getter(reader))); }

            return results;

        }

        /// <summary>Executes a query with no need to capture a return</summary>
        /// <param name="sql">SQL to execute</param>
        /// <returns>Number of rows affected</returns>
        public async Task<int> Execute(string sql) { return await Execute(sql, (_) => { }); }

        /// <summary>Executes a query with no need to capture a return</summary>
        /// <param name="sql">SQL to execute</param>
        /// <param name="setter">Setter for parameters in SQL</param>
        /// <returns>Number of rows affected</returns>
        public async Task<int> Execute(string sql, Action<Setter> setter) {
            using var conn = new NpgsqlConnection(ConnectionString);
            await conn.OpenAsync();

            using var cmd = new NpgsqlCommand(sql, conn);

            void setParam(string key, NpgsqlDbType type, object? val) {
                cmd.Parameters.Add(new NpgsqlParameter(key, type) { Value = val ?? DBNull.Value });
            }

            setter(new Setter(setParam));

            return await cmd.ExecuteNonQueryAsync();
        }

        /// <summary>Executes a batch of queries with no need to capture a return</summary>
        /// <typeparam name="T">Type of items</typeparam>
        /// <param name="sql">Base sql to execute for each item</param>
        /// <param name="setter">Setter for parameters in the SQL using individual items from the collection</param>
        /// <param name="items">Items to use for this batch execution</param>
        /// <returns>Total number of rows affected</returns>
        public async Task<int> ExecuteBatch<T>(string sql, Action<Setter, T> setter, ICollection<T> items) {
            if (items.Count == 0) return 0;

            using var conn = new NpgsqlConnection(ConnectionString);
            await conn.OpenAsync();

            using var batch = new NpgsqlBatch(conn);

            foreach (var item in items) {
                var batchCommand = new NpgsqlBatchCommand(sql);

                void setParam(string key, NpgsqlDbType type, object? val) {
                    batchCommand.Parameters.Add(new NpgsqlParameter(key, type) { Value = val ?? DBNull.Value });
                }

                setter(new Setter(setParam), item);
                batch.BatchCommands.Add(batchCommand);
            }

            return await batch.ExecuteNonQueryAsync();
        }

        /// <summary>Executes a batch of queries with no need to capture a return</summary>
        /// <typeparam name="T">Type of items</typeparam>
        /// <param name="sql">Base sql to execute for each item</param>
        /// <param name="setter">Setter for parameters in the SQL using individual items from the collection</param>
        /// <param name="items">Items to use for this batch execution</param>
        /// <returns>Total number of rows affected</returns>
        public async Task<int> ExecuteBatch<T>(string sql, Func<Setter, T, Task> setter, ICollection<T> items) {
            if (items.Count == 0) return 0;

            using var conn = new NpgsqlConnection(ConnectionString);
            await conn.OpenAsync();

            using var batch = new NpgsqlBatch(conn);

            foreach (var item in items) {
                var batchCommand = new NpgsqlBatchCommand(sql);

                void setParam(string key, NpgsqlDbType type, object? val) {
                    batchCommand.Parameters.Add(new NpgsqlParameter(key, type) { Value = val ?? DBNull.Value });
                }

                await setter(new Setter(setParam), item);
                batch.BatchCommands.Add(batchCommand);
            }

            return await batch.ExecuteNonQueryAsync();
        }
    }
}
