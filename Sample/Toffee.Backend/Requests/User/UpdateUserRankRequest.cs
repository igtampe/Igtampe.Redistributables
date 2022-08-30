namespace Igtampe.Toffee.Backend.Requests.User {

    /// <summary>Request to update a user's rank</summary>
    public class UpdateUserRankRequest {
        /// <summary>Username of the rank to update</summary>
        public string Username { get; set; } = "";

        /// <summary>Rank to update to</summary>
        public int Rank { get; set; } = 0;
    }
}
