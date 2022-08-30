using Igtampe.Exceptions;

namespace Igtampe.Toffee.Common.Exceptions {

    /// <summary>Exception thrown if an operation requires a user to be the assigner, but they are not</summary>
    public class NotTaskAssignerException : ObjectNotOwnedException<Task, Guid> {

        /// <summary>Creates a NotTaskAssignerException</summary>
        /// <param name="ID"></param>
        public NotTaskAssignerException(Guid ID) : base(ID) {}

        /// <summary>Exception's message</summary>
        public override string Message => $"User is not the Assigner of this Task";
    }
}
