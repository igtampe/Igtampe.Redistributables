namespace Igtampe.Toffee.Common.Exceptions {

    /// <summary>Exception thrown when a Task is not assignable (or re-assignable) due to user rank</summary>
    public class TaskUnassignableException : TaskException {

        private readonly int RankAssigner;
        private readonly int RankAssignee;

        /// <summary>Creates a TaskUnassignableException</summary>
        /// <param name="rankAssignee"></param>
        /// <param name="rankAssigner"></param>
        public TaskUnassignableException(int rankAssigner, int rankAssignee) : base(Guid.Empty) {
            RankAssigner = rankAssigner;
            RankAssignee = rankAssignee;
        }

        /// <summary>Message for this exception</summary>
        public override string Message => $"Task cannot be assigned by user with rank {RankAssigner} to user with rank {RankAssignee}";
    }
}
