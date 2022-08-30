using Igtampe.Notifier;

namespace Igtampe.Toffee.Common {
    /// <summary>A Toffee Notification</summary>
    public class Notification : Notification<User>{

        /// <summary>Task associated to this notification</summary>
        public Task? Task { get; set; }
    }
}
