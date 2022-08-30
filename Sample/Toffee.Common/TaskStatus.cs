namespace Igtampe.Toffee.Common {
    /// <summary>Status of a task</summary>
    public enum TaskStatus {

        /// <summary>A Task has been re-assigned to a different </summary>
        REASSIGNED = -1,

        /// <summary>A Task has been assigned to the assignee by the assigner</summary>
        ASSIGNED = 0,

        /// <summary>A task has been accepted by the assignee</summary>
        ACCEPTED = 1,

        /// <summary>A task has been rejected by the assignee</summary>
        REJECTED = 2,

        /// <summary>A task has begun to be worked on by the assignee, and is actively being worked on</summary>
        IN_PROGRESS = 3,

        /// <summary>A task has begun to be worked on by the assignee, but is on-hold, and can be resumed at any moment</summary>
        ON_HOLD = 4,

        /// <summary>A task has begun to be worked on by the assignee, but is blocked by another task</summary>
        BLOCKED = 5,

        /// <summary>A task has been completed by the assignee</summary>
        COMPLETED = 6,

        /// <summary>A task has been dropped by the assignee</summary>
        DROPPED = 7,

        /// <summary>A task has been canceled by the assigner</summary>
        CANCELLED = 8,
    }
}
