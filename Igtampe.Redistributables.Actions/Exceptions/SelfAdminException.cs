using Igtampe.ChopoAuth.Exceptions;

namespace Igtampe.Actions.Exceptions {
    
    /// <summary>Exception that's thrown if a User tries to modify their own admin status</summary>
    public class SelfAdminException : UserException {
        
        /// <summary>Creates a NoAdminsException</summary>
        /// <param name="Username"></param>
        public SelfAdminException(string Username) : base(Username) {}

        /// <summary>Message for this exception</summary>
        public override string Message => $"Changing admin status cannot be done on self";

    }
}
