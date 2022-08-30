using Igtampe.Exceptions;

namespace Igtampe.Toffee.Common.Exceptions {

    /// <summary>Exception thrown if an operation requires a user to be the assignee, but they are not</summary>
    public class NotTaskAssigneeException : ObjectNotOwnedException<Task, Guid> {

        /// <summary>Creates a NotTaskAssigneeException</summary>
        /// <param name="ID"></param>
        public NotTaskAssigneeException(Guid ID) : base(ID) {}

        /// <summary>Exception's message</summary>
        public override string Message => $"User is not the Assignee of this Task";
    }
}
