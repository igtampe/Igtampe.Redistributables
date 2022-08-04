using Igtampe.Exceptions;

namespace Igtampe.ChopoAuth.Exceptions {
    /// <summary>An Exception relating to a ChopoAuth user</summary>
    public class UserException : ObjectException<User,string>{
        
        /// <summary>Creates a UserException relating to the given username</summary>
        /// <param name="Username"></param>
        public UserException(string Username) : base(Username) { }

        /// <summary>Creates a UserException relating to the given Username, and a different message</summary>
        /// <param name="Username"></param>
        /// <param name="Message"></param>
        public UserException(string Username, string? Message) : base(Username, Message) { }
    }
}
