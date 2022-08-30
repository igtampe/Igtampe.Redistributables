using Igtampe.Toffee.Common;

namespace Igtampe.Toffee.Backend.Requests.Task {
    public class TaskAssignerRequest {

        public string Username { get; set; } = "";

        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        public DateTime DueDate { get; set; } = DateTime.MinValue;

        public TaskPriority Priority { get; set; } = TaskPriority.NONE;

        public Guid? CategoryID { get; set; }

        public bool Notify { get; set; } = false;

    }
}
