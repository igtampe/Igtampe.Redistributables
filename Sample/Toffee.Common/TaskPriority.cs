namespace Igtampe.Toffee.Common {

    /// <summary>Priority of a task</summary>
    public enum TaskPriority {

        /// <summary>A task has no priority assigned</summary>
        NONE = -1,

        /// <summary>A task currently in the backlog that can be dealt with Later(TM)</summary>
        BACKLOG = 0,

        /// <summary>A Task with very low priority</summary>
        VERY_LOW = 1,

        /// <summary>A Task with low priority</summary>
        LOW = 2,

        /// <summary>A Task with medium priority</summary>
        MEDIUM = 3,
        
        /// <summary>A Task with high priority</summary>
        HIGH = 4,

        /// <summary>A task that has very high priority</summary>
        VERY_HIGH = 5,

        /// <summary>A task that has to be tended to RIGHT NOW</summary>
        STAT = 6,

    }
}
