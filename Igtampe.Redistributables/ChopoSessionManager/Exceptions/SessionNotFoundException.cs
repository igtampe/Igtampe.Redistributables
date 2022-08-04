using Igtampe.Exceptions;

namespace Igtampe.ChopoSessionManager.Exceptions {

    /// <summary>An Exception that indicates the requested session was not found, where one is required to proceed</summary>
    public class SessionNotFoundException : ObjectNotFoundException<Session,Guid> {

        /// <summary>Creates a SessionNotFound exception indicating the given ID was not found</summary>
        /// <param name="ID"></param>
        public SessionNotFoundException(Guid ID) : base(ID) {}
    }
}
