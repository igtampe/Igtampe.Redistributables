namespace Igtampe.Toffee.Backend.Requests.Task {
    public class TaskAssigneeRequest {

        public Common.TaskStatus Status { get; set; }  = Common.TaskStatus.ACCEPTED;

        public string StatusMessage { get; set; } = "";

        public bool Notify { get; set; } = false;

    }
}
