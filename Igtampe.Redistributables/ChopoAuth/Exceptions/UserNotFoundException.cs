using Igtampe.Exceptions;

namespace Igtampe.ChopoAuth.Exceptions {
    /// <summary>An exception that occurs when a User was not found</summary>
    public class UserNotFoundException : ObjectNotFoundException<User,string> {

        /// <summary>Creates a UserNotFoundException when the given username was not found</summary>
        /// <param name="Username"></param>
        public UserNotFoundException(string Username) : base(Username) {}
    }
}
