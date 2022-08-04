using Igtampe.Exceptions;

namespace Igtampe.Notifier.Exceptions {

    /// <summary>An Exception that indicates the requested Notification was not found, where one is required to proceed</summary>
    public class NotificationNotFoundException : ObjectNotFoundException<Notification,Guid> {

        /// <summary>Creates a NotificationNotFound exception indicating the given ID was not found</summary>
        /// <param name="ID"></param>
        public NotificationNotFoundException(Guid ID) : base(ID) {}
    }
}
