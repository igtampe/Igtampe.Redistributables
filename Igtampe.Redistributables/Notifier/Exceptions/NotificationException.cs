using Igtampe.Exceptions;

namespace Igtampe.Notifier.Exceptions {

    /// <summary>An Exception relating to a Notification from Notifier</summary>
    public class NotificationException : ObjectException<Notification,Guid> {

        /// <summary>Creates a NotificationException for a notification with given ID</summary>
        /// <param name="ID"></param>
        public NotificationException(Guid ID) : base(ID) { }

        /// <summary>Creates a NotificationException relating to the given Notification, and a different message</summary>
        /// <param name="ID"></param>
        /// <param name="Message"></param>
        public NotificationException(Guid ID, string? Message) : base(ID, Message) { }
    }
}
