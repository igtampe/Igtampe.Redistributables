using Igtampe.Exceptions;

namespace Igtampe.ChopoSessionManager.Exceptions {

    /// <summary>An Exception relating to a Session from the Chopo Session Manager</summary>
    public class SessionException : ObjectException<Session,Guid>{

        /// <summary>Creates a SessionException for a session with given ID</summary>
        /// <param name="ID"></param>
        public SessionException(Guid ID) :base(ID) { }

        /// <summary>Creates a SessionException relating to the given Session, and a different message</summary>
        /// <param name="ID"></param>
        /// <param name="Message"></param>
        public SessionException(Guid ID, string? Message) : base(ID, Message) { }
    }
}
