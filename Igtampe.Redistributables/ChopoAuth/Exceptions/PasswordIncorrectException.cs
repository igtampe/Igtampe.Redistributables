namespace Igtampe.ChopoAuth.Exceptions {

    /// <summary>Exception that occurs when password verification fails and is required to proceed</summary>
    public class PasswordIncorrectException : UserException {

        /// <summary>Creates a PasswordIncorrectException for given username</summary>
        /// <param name="Username"></param>
        public PasswordIncorrectException(string Username) : base(Username) {}

        /// <summary>Message for this exception</summary>
        public override string Message => $"Password for '{ID}' was incorrect";
    }
}
