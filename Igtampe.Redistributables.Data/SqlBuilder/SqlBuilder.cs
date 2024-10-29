namespace Igtampe.Data.SqlBuilder {
    
    /// <summary>Basic parametrized SQL statement constructor</summary>
    public class SqlBuilder {

        private enum SqlOperation { SELECT, INSERT, UPDATE, DELETE }

        /// <summary>Comparison operator for a condition</summary>
        public enum WhereConditionOperator {

            /// <summary>=</summary>
            EQUALS, 
            
            /// <summary>></summary>
            GREATER_THAN, 
            
            /// <summary>Less than </summary>
            LESS_THAN, 
            
            /// <summary>>=</summary>
            GREATER_OR_EQUAL, 
            
            /// <summary>Less or equal</summary>
            LESS_OR_EQUAL, 
            
            /// <summary>IN (should be used with the list where condition)</summary>
            IN, 
            
            /// <summary>NOT =</summary>
            NOT_EQUALS, 
            
            /// <summary>NOT in</summary>
            NOT_IN, 
            
            /// <summary>LIKE for strings</summary>
            LIKE, 
            
            /// <summary>Case insensitive LIKE</summary>
            ILIKE, 
            
            /// <summary>NOT like</summary>
            NOT_LIKE, 
            
            /// <summary>NOT case insensitive LIKE</summary>
            NOT_ILIKE
        };

        /// <summary>Union for multiple conditions</summary>
        public enum WhereConditionUnion {
            
            /// <summary>AND union</summary>
            AND, 
            
            /// <summary>OR union</summary>
            OR
        };

        /// <summary>Order to sort a column by</summary>
        public enum SortOrder {
            /// <summary>Sorts by Ascending</summary>
            ASC, 
            /// <summary>Sorts by Descending</summary>
            DESC
        }

        /// <summary>Group of where conditions</summary>
        /// <param name="union">Whether this is an AND or OR condition group</param>
        /// <param name="conditions">Conditions to check</param>
        public class WhereConditionGroup(WhereConditionUnion union, List<WhereCondition> conditions) {

            /// <summary>Creates a where condition group with AND as union</summary>
            /// <param name="conditions"></param>
            public WhereConditionGroup(List<WhereCondition> conditions) : this(WhereConditionUnion.AND, conditions) { }

            /// <summary>Returns a WHERE condition string</summary>
            /// <returns></returns>
            public override string ToString() {
                return string.Join(union switch {
                    WhereConditionUnion.AND => " AND ",
                    WhereConditionUnion.OR => " OR ",
                    _ => ""
                }, conditions.Select(a => a.ToString()));
            }
        }

        /// <summary>Subgroup of conditions placed in parenthesis</summary>
        /// <param name="group"></param>
        public class WhereConditionSubgroup(WhereConditionGroup group) : WhereCondition("") {

            /// <summary>Returns the WHERE condition group contained in a parenthesis</summary>
            /// <returns></returns>
            public override string ToString() {
                return "(" + group.ToString() + ")";
            }
        }

        /// <summary>Join condition on two tables</summary>
        /// <param name="table1Alias">Alias of the first table</param>
        /// <param name="table2Alias">Alias of the second table</param>
        /// <param name="joinColumn">Column to join on</param>
        public class JoinCondition(string table1Alias, string table2Alias, string joinColumn)
            : WhereCondition($"{table1Alias}.{joinColumn}", WhereConditionOperator.EQUALS, $"{table2Alias}.{joinColumn}") { }

        /// <summary>Where condition</summary>
        /// <param name="column">Column to operate on</param>
        /// <param name="operation">Comparison operation</param>
        /// <param name="value">Value to compare against</param>
        public class WhereCondition(string column, WhereConditionOperator operation, string value) {

            /// <summary>Simple condition where the column equals the parametrized input</summary>
            /// <param name="column"></param>
            public WhereCondition(string column) : this(column, WhereConditionOperator.EQUALS, $"@{column}") { }

            /// <summary>Simple condition where the column matches the operation on the parametrized input</summary>
            /// <param name="column"></param>
            /// <param name="operation"></param>
            public WhereCondition(string column, WhereConditionOperator operation) : this(column, operation, $"@{column}") { }

            /// <summary>Condition in which the value on the column is in the list of given values</summary>
            /// <param name="column"></param>
            /// <param name="vals"></param>
            public WhereCondition(string column, List<string> vals) : this(column, WhereConditionOperator.IN,
                "(" + string.Join(",", vals) + ")"
                ) { }

            /// <summary>Returns the WHERE condition string for this condition</summary>
            /// <returns></returns>
            /// <exception cref="NotImplementedException">If the WHERE Condition Operator is not set</exception>
            public override string ToString() => @$"{column} {operation switch {
                WhereConditionOperator.EQUALS => "=",
                WhereConditionOperator.NOT_EQUALS => "!=",
                WhereConditionOperator.GREATER_THAN => ">",
                WhereConditionOperator.LESS_THAN => "<",
                WhereConditionOperator.GREATER_OR_EQUAL => ">=",
                WhereConditionOperator.LESS_OR_EQUAL => "<=",
                WhereConditionOperator.IN => "IN",
                WhereConditionOperator.NOT_IN => "NOT IN",
                WhereConditionOperator.ILIKE => "ILIKE",
                WhereConditionOperator.NOT_ILIKE => "NOT ILIKE",
                WhereConditionOperator.LIKE => "LIKE",
                WhereConditionOperator.NOT_LIKE => "NOT LIKE",
                _ => throw new NotImplementedException(),
            }} {value}";

        }

        /// <summary>How to order the results</summary>
        /// <param name="column">Column to order by</param>
        /// <param name="order">Whether to order ASC or DESC</param>
        public class OrderByColumn(string column, SortOrder order) {

            /// <summary>Creates an order by column by ASC</summary>
            /// <param name="column"></param>
            public OrderByColumn(string column) : this(column, SortOrder.ASC) { }

            /// <summary>Returns an ORDER BY string</summary>
            /// <returns></returns>
            /// <exception cref="NotImplementedException"></exception>
            public override string ToString() => $"{column} {order switch {
                SortOrder.ASC => "ASC",
                SortOrder.DESC => "DESC",
                _ => throw new NotImplementedException(),
            }}";

        }

        private SqlBuilder() { }

        private SqlBuilder(SqlOperation operation, List<string> columns, string table) {
            _operation = operation;
        }

        private SqlOperation _operation = SqlOperation.SELECT;
        private string _table = "";
        private List<string> _columns = [];
        private WhereConditionGroup? _where = null ;
        private List<OrderByColumn> _orderBy = [];
        private List<string> _groupBy = [];
        private Dictionary<string, string> _setValues = [];
        private List<string> _returning = [];
        private int? _limit = null;
        private int? _offset = null;
        private bool _distinct = false;


        /// <summary>Builds a SELECT query for given columns and table</summary>
        /// <param name="columns">Columns to read</param>
        /// <param name="from">Table to read from. Can be multiple tables</param>
        /// <returns></returns>
        public static SqlBuilder Select(List<string> columns, string from) {
            return new(SqlOperation.SELECT, columns, from);   
        }


        /// <summary>Builds an INSERT query for the given tables and columns</summary>
        /// <param name="into">Table this operation will be done on</param>
        /// <param name="columns">Columns that will be set. If a value isn't defined through <see cref="SetValues(Dictionary{string, string})"/>, it will be parametrized</param>
        /// <returns></returns>
        public static SqlBuilder Insert(string into, List<string> columns) {
            return new(SqlOperation.INSERT, columns, into);
        }


        /// <summary>Builds an UPDATE query for given table and columns</summary>
        /// <param name="table">Table this operation will be done on</param>
        /// <param name="columns">Columns that will be set. If a value isn't defined through <see cref="SetValues(Dictionary{string, string})"/>, it will be parametrized</param>
        /// <returns></returns>
        public static SqlBuilder Update(string table, List<string> columns) { 
            return new(SqlOperation.UPDATE, columns, table);
            
        }


        /// <summary>Builds a base DELETE operation</summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public static SqlBuilder Delete(string from) {
            return new(SqlOperation.DELETE, [], from);
        }

        /// <summary>Where conditions for this call</summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public SqlBuilder Where(WhereConditionGroup conditions) {
            _where = conditions;
            return this;
        }

        /// <summary>Where condition for this call</summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public SqlBuilder Where(WhereCondition condition) {
            _where = new([condition]);
            return this;
        }

        /// <summary>Columns to order by</summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public SqlBuilder OrderBy(List<OrderByColumn> columns) {
            _orderBy = columns;
            return this;
        }

        /// <summary>Sets a group by condition</summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public SqlBuilder GroupBy(List<string> columns) {
            _groupBy = columns;
            return this;
        }

        /// <summary>Specifies values for an INSERT or UPDATE query that should not be parametrized</summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public SqlBuilder SetValues(Dictionary<string, string> values) {
            _setValues = values;
            return this;
        }

        /// <summary>Mark this query to return distinct values</summary>
        /// <returns></returns>
        public SqlBuilder Distinct() {
            _distinct = true;
            return this;
        }

        /// <summary>Maximum number of rows to return</summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        public SqlBuilder Limit(int limit) {
            _limit = limit;
            return this;
        }

        /// <summary>Offset from initial row from this return</summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public SqlBuilder Offset(int offset) {
            if (_offset == 0) return this; //No need to set if the offset is 0
            _limit = offset;
            return this;
        }

        /// <summary>Columns to return on a INSERT or UPDATE query</summary>
        /// <param name="columns"></param>
        /// <returns></returns>
        public SqlBuilder Returning(List<string> columns) {
            _returning = columns;
            return this;
        }



        /// <summary>Returns the built SQL string</summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override string ToString() {
            return _operation switch {
                SqlOperation.SELECT => BuildSelect(),
                SqlOperation.INSERT => BuildInsert(),
                SqlOperation.UPDATE => BuildUpdate(),
                SqlOperation.DELETE => BuildDelete(),
                _ => throw new NotImplementedException(),
            };
        }

        private string BuildSelect() {
            return @$"
SELECT {(_distinct ? "DISTINCT" : "")} {string.Join(",", _columns)}
FROM {_table} 
{(_where!= null ? $"WHERE {_where} " : "")}
{(_groupBy.Count > 0 ? $"GROUP BY {string.Join(", ", _groupBy)} ": "")}
{(_orderBy.Count > 0 ? $"ORDER BY {string.Join(", ", _orderBy.Select(a => a.ToString()))} " : "")}
{(_limit != null ? $"LIMIT {_limit} " : "")}
{(_offset != null ? $"OFFSET {_offset} " : "")}
";
        }

        private string BuildInsert() {
            return $@"
INSERT INTO {_table} ({string.Join(",", _columns)})
VALUES ({string.Join(",", _columns.Select(a => _setValues.GetValueOrDefault(a) ?? $"@{a}"))})
{(_returning.Count==0 ? "" : $"RETURNING {string.Join(", ",_returning)}")}
";
        }

        private string BuildUpdate() {
            return $@"
UPDATE {_table} SET 
{string.Join(",", _columns.Select(a => $"{a}={_setValues.GetValueOrDefault(a) ?? $"@{a}"}"))}
{(_where != null ? $"WHERE {_where} " : "")}
{(_returning.Count == 0 ? "" : $"RETURNING {string.Join(", ", _returning)}")}
";
        }

        private string BuildDelete() {
            return $@"
DELETE FROM {_table} 
{(_where != null ? $"WHERE {_where} " : "")}
";
        }

    }
}
