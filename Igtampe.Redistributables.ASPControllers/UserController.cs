using Microsoft.AspNetCore.Mvc;
using Igtampe.ChopoSessionManager;
using Microsoft.EntityFrameworkCore;
using Igtampe.DBContexts;
using Igtampe.ChopoAuth;
using Igtampe.Controllers.Requests;
using Igtampe.Actions;

namespace Igtampe.Controllers {

    /// <summary>Controller that handles User operations</summary>
    public class UserController<E,F> : ErrorResultControllerBase where E : DbContext, IUserContext<F> where F : User, new(){

        private readonly AuthAgent<E,F> Agent;

        /// <summary>Creates a User Controller</summary>
        /// <param name="Context"></param>
        /// <param name="Manager">Session Manager used by this controller. If null, it'll use the default singleton one from <see cref="SessionManager.Manager"/> </param>
        public UserController(E Context, ISessionManager? Manager = null) 
            => Agent = new(Context, Manager ?? SessionManager.Manager);

        #region Gets
        /// <summary>Gets a directory of all users</summary>
        /// <param name="Query">Search query to look for in Username and Name</param>
        /// <param name="Take">Amount of users to take from the list</param>
        /// <param name="Skip">Amount of users to skip over when taking from the list</param>
        /// <returns></returns>
        [HttpGet("Dir")]
        public async Task<IActionResult> Directory([FromQuery] string? Query, [FromQuery] int? Take, [FromQuery] int? Skip)
            => Ok(await Agent.GetDirectory(Query, Take, Skip));

        /// <summary>Gets username of the currently logged in session</summary>
        /// <param name="SessionID">ID of the session</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetMe([FromHeader] Guid? SessionID) 
            => Ok(await Agent.GetMe(SessionID));

        /// <summary>Gets a given user</summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet("{ID}")]
        public async Task<IActionResult> GetUser(string ID) 
            => Ok(await Agent.GetUser(ID));

        #endregion

        #region Puts

        /// <summary>Handles user password changes</summary>
        /// <param name="Request">Request with their current and new password</param>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <returns></returns>
        // PUT api/Users
        [HttpPut]
        public async Task<IActionResult> Update([FromHeader] Guid? SessionID, [FromBody] ChangePasswordRequest Request) =>
            Request.New is null || Request.Current is null ? BadRequest("Cannot have empty passwords") //Ensure nothing is null
            : Ok(await Agent.ChangePassword(SessionID, Request.New, Request.Current));

        /// <summary>Request to reset the password of a user</summary>
        /// <param name="SessionID">SessionID of an administrator who wishes to change the password of another user</param>
        /// <param name="ID">ID of the user to change the password of</param>
        /// <param name="Request">Request to change</param>
        /// <returns></returns>
        [HttpPut("{ID}/Reset")]
        public async Task<IActionResult> ResetPassword([FromHeader] Guid? SessionID, [FromRoute] string ID, [FromBody] ChangePasswordRequest Request) =>
            Request.New is null || Request.Current is null //Ensure nothing is null
                ? BadRequest("Cannot have empty password") 
                : Ok(await Agent.ResetPassword(SessionID, ID, Request.New));

        /// <summary>Updates the image of the user with this session</summary>
        /// <param name="SessionID"></param>
        /// <param name="ImageURL"></param>
        /// <returns></returns>
        [HttpPut("image")]
        public async Task<IActionResult> UpdateImage([FromHeader] Guid? SessionID, [FromBody] string ImageURL) 
            => Ok(await Agent.SetImage(SessionID, ImageURL));

        #endregion

        #region Posts

        // POST api/Users
        /// <summary>Handles logging in to Clothespin</summary>
        /// <param name="Request">Request with a User and Password attempt to log in</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> LogIn(UserRequest Request) => 
            Request.Username is null || Request.Password is null 
                ? BadRequest("User or Password was empty") 
                : Ok(new { SessionID = await Agent.LogIn(Request.Username, Request.Password) });

        /// <summary>Handles user registration</summary>
        /// <param name="Request">User and password combination to create</param>
        /// <returns></returns>
        // POST api/Users/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest Request) => 
            Request.Username is null || Request.Password is null 
                ? BadRequest("User or Password was empty") 
                : Ok(await Agent.CreateUser(Request.Username, Request.Name, Request.Password));

        /// <summary>Handles user logout</summary>
        /// <param name="SessionID">Session to log out of</param>
        /// <returns></returns>
        // POST api/Users/out
        [HttpPost("out")]
        public async Task<IActionResult> LogOut([FromHeader] Guid SessionID) 
            => Ok(await Agent.LogOut(SessionID));

        /// <summary>Handles user logout of *all* sessions</summary>
        /// <param name="SessionID">Session that wants to log out of all tied sessions</param>
        /// <returns></returns>
        // POST api/Users/outall
        [HttpPost("outall")]
        public async Task<IActionResult> LogOutAll([FromHeader] Guid SessionID) =>
            Ok(await Agent.LogOutAll(SessionID));

        #endregion

    }
}
