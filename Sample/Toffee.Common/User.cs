using Igtampe.ChopoImageHandling;

namespace Igtampe.Toffee.Common {

    /// <summary>A Toffee User</summary>
    public class User : ChopoAuth.User {

        //Notice how there's nothing here involving Auth. All of that is taken care of by ChopoAuth.
        //Here only goes any special things involving this application

        /// <summary>Rank of this user, which will be used to determine if a user can assign tasks</summary>
        public int Rank { get; set; } = 0;

        /// <summary>Profile Picture image of the User (Overrides ImageURL)</summary>
        public Image? ProfilePicture { get; set; }
    }
}
