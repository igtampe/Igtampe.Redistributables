namespace Igtampe.Toffee.Common {

    /// <summary>Static utils class to check if transitions are valid or not</summary>
    public static class TaskStateTransitionChecker {

        /// <summary>The executor of the Task State Transition</summary>
        public enum Executor {
            /// <summary>Administrator executor (they can do anything)</summary>
            Admin = -1,

            /// <summary>Person that was assigned the task</summary>
            Assignee = 0,

            /// <summary>Person that assigned the task</summary>
            Assigner = 1,
        };

        /// <summary>Dictionary of valid transitions and executors of that transition</summary>
        private static readonly Dictionary<TaskStatus, List<Tuple<TaskStatus, Executor>>> VALID_TRANSITIONS = new() {

            { TaskStatus.ASSIGNED, new(){ 
                new(TaskStatus.ACCEPTED,Executor.Assignee),
                new(TaskStatus.REJECTED,Executor.Assignee),
                new(TaskStatus.CANCELLED,Executor.Assigner)
            }},
            
            { TaskStatus.REASSIGNED, new(){
                new(TaskStatus.ACCEPTED,Executor.Assignee),
                new(TaskStatus.REJECTED,Executor.Assignee),
                new(TaskStatus.CANCELLED,Executor.Assigner)
            }},

            { TaskStatus.ACCEPTED, new(){
                new(TaskStatus.IN_PROGRESS,Executor.Assignee),
                new(TaskStatus.ON_HOLD,Executor.Assignee),
                new(TaskStatus.BLOCKED,Executor.Assignee),
                new(TaskStatus.COMPLETED,Executor.Assignee),
                new(TaskStatus.DROPPED,Executor.Assignee),
                new(TaskStatus.CANCELLED,Executor.Assigner)
            }},

            { TaskStatus.IN_PROGRESS, new(){
                new(TaskStatus.ON_HOLD,Executor.Assignee),
                new(TaskStatus.BLOCKED,Executor.Assignee),
                new(TaskStatus.COMPLETED,Executor.Assignee),
                new(TaskStatus.DROPPED,Executor.Assignee),
                new(TaskStatus.CANCELLED,Executor.Assigner)
            }},

            { TaskStatus.ON_HOLD, new(){
                new(TaskStatus.IN_PROGRESS,Executor.Assignee),
                new(TaskStatus.BLOCKED,Executor.Assignee),
                new(TaskStatus.COMPLETED,Executor.Assignee),
                new(TaskStatus.DROPPED,Executor.Assignee),
                new(TaskStatus.CANCELLED,Executor.Assigner)
            }},

            { TaskStatus.BLOCKED, new(){
                new(TaskStatus.ON_HOLD,Executor.Assignee),
                new(TaskStatus.IN_PROGRESS,Executor.Assignee),
                new(TaskStatus.COMPLETED,Executor.Assignee),
                new(TaskStatus.DROPPED,Executor.Assignee),
                new(TaskStatus.CANCELLED,Executor.Assigner)
            }},

            { TaskStatus.COMPLETED, new(){}},
            { TaskStatus.CANCELLED, new(){}},

            { TaskStatus.DROPPED, new(){
                new(TaskStatus.REASSIGNED,Executor.Assigner)
            }},

        };

        /// <summary>Checks if a transition is valid or not</summary>
        /// <param name="State1">Current state</param>
        /// <param name="Stat2">State to transition to</param>
        /// <param name="Exec">Type of user executing this transition</param>
        /// <returns>True if the transition is valid, false otherwise</returns>
        public static bool CheckStateTransition(TaskStatus State1, TaskStatus Stat2, Executor Exec) 
            => Exec == Executor.Admin || VALID_TRANSITIONS[State1].Contains(new(Stat2, Exec));
    }
}
