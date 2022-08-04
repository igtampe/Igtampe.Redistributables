namespace Igtampe.Controllers.Requests {
    /// <summary>Request to register a user</summary>
    public class RegisterRequest : UserRequest {

        /// <summary>Name of this user to be registered </summary>
        public string Name { get; set; } = "";
    }
}
