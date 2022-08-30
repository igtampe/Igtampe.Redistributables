namespace Igtampe.Toffee.Common {

    /// <summary>Order of tasks</summary>
    public enum TaskOrder {

        /// <summary>Order by closest first</summary>
        DUE_DATE = 0,

        /// <summary>Order by the date this task was assigned</summary>
        ASSIGNED_DATE = 1,

        /// <summary>Order by highest priority</summary>
        PRIORITY = 2,

        /// <summary>Order by name (?)</summary>
        NAME = 3,
    }
}
