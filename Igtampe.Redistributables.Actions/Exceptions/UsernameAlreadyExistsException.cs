using Igtampe.ChopoAuth.Exceptions;

namespace Igtampe.Actions.Exceptions {

    /// <summary>Exception thrown when a Username already exists in the database</summary>
    public class UsernameAlreadyExistsException : UserException {

        /// <summary>Creates a UsernameAlreadyExists Exception</summary>
        /// <param name="Username"></param>
        public UsernameAlreadyExistsException(string Username) : base(Username) {}

        /// <summary>Message of this exception</summary>
        public override string Message => $"User \'{ID}\' already exists!";
    }
}
