namespace Igtampe.ChopoAuth.Exceptions {

    /// <summary>An Exception when a User is missing required roles to commit an action</summary>
    public class UserRolesException : UserException {

        /// <summary>Roles this user is missing that are required to commit this action</summary>
        public string RolesRequired { get; set; }

        /// <summary>Creates a UserRolesException due to the given username missing the given roles</summary>
        /// <param name="Username"></param>
        /// <param name="RolesRequired"></param>
        public UserRolesException(string Username, string RolesRequired) : base(Username) => this.RolesRequired = RolesRequired;

        /// <summary>Message relating to this exception</summary>
        public override string Message => $"User '{ID}' is missing role(s) '{RolesRequired}'";
    }
}
