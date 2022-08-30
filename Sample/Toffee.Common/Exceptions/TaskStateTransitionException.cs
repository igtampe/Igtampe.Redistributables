using static Igtampe.Toffee.Common.TaskStateTransitionChecker;

namespace Igtampe.Toffee.Common.Exceptions {

    /// <summary>Exception thrown when a requested task state transition is not valid</summary>
    public class TaskStateTransitionException : TaskException {

        private readonly Common.TaskStatus Current;
        private readonly Common.TaskStatus New;
        private readonly Executor Executor;

        /// <summary>Creates a Task State Transition Exception</summary>
        /// <param name="ID"></param>
        /// <param name="Current"></param>
        /// <param name="New"></param>
        /// <param name="Executor"></param>
        public TaskStateTransitionException(Guid ID, Common.TaskStatus Current, Common.TaskStatus New, Executor Executor) : base(ID) {
            this.Current = Current;
            this.New = New;
            this.Executor = Executor;
        }

        /// <summary>Message for this exception</summary>
        public override string Message => $"Cannot transition from {Current} to {New} as an {Executor}";

    }
}
