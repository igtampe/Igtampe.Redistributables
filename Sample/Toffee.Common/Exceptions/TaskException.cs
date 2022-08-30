using Igtampe.Exceptions;

namespace Igtampe.Toffee.Common.Exceptions {

    /// <summary>Exception thrown relating to a Task</summary>
    public class TaskException : ObjectException<Common.Task, Guid> {

        /// <summary>Creates a Task Exception for Task with given ID</summary>
        /// <param name="ID"></param>
        public TaskException(Guid ID) : base(ID) {}

        /// <summary>Creates a Task Exception with a custom message</summary>
        /// <param name="ID"></param>
        /// <param name="Message"></param>
        public TaskException(Guid ID, string? Message) : base(ID, Message) {}
    }
}
