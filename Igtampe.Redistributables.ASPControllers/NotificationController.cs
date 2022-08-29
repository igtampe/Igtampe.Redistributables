using Microsoft.AspNetCore.Mvc;
using Igtampe.ChopoSessionManager;
using Microsoft.EntityFrameworkCore;
using Igtampe.DBContexts;
using Igtampe.ChopoAuth;
using Igtampe.Actions;

namespace Igtampe.Controllers {

    /// <summary>Controller that handles User operations</summary>
    public class NotificationController<E,F> : ErrorResultControllerBase where E : DbContext, IUserContext<F>, INotificationContext<F> where F : User, new() {

        private readonly NotificationAgent<E,F> Agent;

        /// <summary>Creates a User Controller</summary>
        /// <param name="Context"></param>
        /// <param name="Manager">Optional session manager</param>
        public NotificationController(E Context, ISessionManager? Manager = null) 
            => Agent = new(Context, Manager ?? SessionManager.Manager);

        /// <summary>Gets all notifications from the logged in user</summary>
        /// <param name="SessionID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromHeader] Guid? SessionID)
            => Ok(await Agent.GetAll(SessionID));

        /// <summary>Deletes one notification from the logged in user</summary>
        /// <param name="SessionID"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpDelete("{ID}")]
        public async Task<IActionResult> DeleteOne([FromHeader] Guid? SessionID, [FromRoute] Guid ID) {
            await Agent.DeleteOne(SessionID,ID);
            return Ok();
        }

        /// <summary>Deletes all notifications from the logged in user</summary>
        /// <param name="SessionID"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteAll([FromHeader] Guid? SessionID) {
            await Agent.DeleteAll(SessionID);
            return Ok();
        }
    }
}
