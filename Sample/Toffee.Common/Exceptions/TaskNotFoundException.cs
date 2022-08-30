using Igtampe.Exceptions;

namespace Igtampe.Toffee.Exceptions {
    
    /// <summary>Exception thrown when a Task is not found</summary>
    public class TaskNotFoundException : ObjectNotFoundException<Common.Task, Guid> {
    
        /// <summary>Creates a Task Not Found exception</summary>
        /// <param name="ID"></param>
        public TaskNotFoundException(Guid ID) : base(ID) {}
    }
}
