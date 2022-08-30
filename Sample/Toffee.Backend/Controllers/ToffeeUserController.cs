using Igtampe.ChopoImageHandling;
using Igtampe.ChopoSessionManager;
using Igtampe.Controllers;
using Igtampe.Toffee.Actions;
using Igtampe.Toffee.Backend.Requests.User;
using Igtampe.Toffee.Common;
using Igtampe.Toffee.Data;
using Microsoft.AspNetCore.Mvc;

namespace Igtampe.Toffee.Backend.Controllers {

    [Route("API/Users")]
    [ApiController]
    public class ToffeeUserController : UserController<ToffeeContext,User>  {

        private readonly UserAgent Agent;

        public ToffeeUserController(ToffeeContext Context) : base(Context, SessionManager.Manager) 
            => Agent = new(Context, SessionManager.Manager);

        [HttpPut("/Rank")]
        public async Task<IActionResult> UpdateUserRank([FromHeader] Guid? SessionID, [FromBody] UpdateUserRankRequest Request)
            => Ok(await Agent.UpdateUserRank(SessionID, Request.Username, Request.Rank));

        [HttpPut("/Profile")]
        public async Task<IActionResult> UpdateUserProfile([FromHeader] Guid? SessionID) {

            //Get the image from the body
            Image I = await ControllerUtils.GetImageFromRequest(Request);

            //update the thing with the agent
            return Ok(await Agent.UpdateUserProfile(SessionID,I));

        }
    }
}
