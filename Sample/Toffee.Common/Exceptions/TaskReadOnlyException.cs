using Igtampe.Exceptions;

namespace Igtampe.Toffee.Common.Exceptions {

    /// <summary>Exception thrown when a Task that is Read-Only is attempted to be edited</summary>
    public class TaskReadOnlyException : ObjectException<Task, Guid> {

        /// <summary>Creates a TaskReadOnlyException</summary>
        /// <param name="ID"></param>
        public TaskReadOnlyException(Guid ID) : base(ID) {}

        /// <summary>Message for this exception</summary>
        public override string Message => $"Task {ID} is readonly and cannot be edited";
    }
}
