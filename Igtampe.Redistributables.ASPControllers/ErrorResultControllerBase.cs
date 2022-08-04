using Igtampe.Actions;
using Microsoft.AspNetCore.Mvc;

namespace Igtampe.Controllers {

    /// <summary>
    /// Controller base with shortcuts to the ErrorResult returns<br/><br/>
    /// 
    /// This'll still work if you want to handle exceptions or errors at the individual controller level, but its more recommended 
    /// to use a global exception handler with custom exceptions thrown either at the Controller level, or at the <see cref="ActionAgent{E}"/> level.
    /// </summary>
    public class ErrorResultControllerBase : ControllerBase {

        /// <summary>400 Bad Request</summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        [NonAction]
        protected BadRequestObjectResult BadRequest(string Message) => base.BadRequest(ErrorResult.BadRequest(Message));

        /// <summary>401 Unauthorized: An invalid session has been provided</summary>
        /// <returns></returns>
        [NonAction]
        protected UnauthorizedObjectResult InvalidSession() => base.Unauthorized(ErrorResult.Reusable.InvalidSession);

        /// <summary>401 Unauthorized</summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        [NonAction]
        protected UnauthorizedObjectResult Unauthorized(string Message) => base.Unauthorized(ErrorResult.Unauthorized(Message));

        /// <summary>403 Forbidden: Missing specified roles.<br/><br/>NOTE: Due to Forbid(), these are sent back as 401 Unauthorized</summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        [NonAction]
        protected UnauthorizedObjectResult ForbiddenRoles(params string[] Message) => base.Unauthorized(ErrorResult.ForbiddenRoles(string.Join(", ",Message)));

        /// <summary>403 Unauthorized: Some other reason<br/><br/>NOTE: Due to Forbid(), these are sent back as 401 Unauthorized</summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        [NonAction]
        protected UnauthorizedObjectResult Forbidden(string Message) => base.Unauthorized(ErrorResult.Forbidden(Message));

        /// <summary>404 Not Found</summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        [NonAction]
        protected NotFoundObjectResult NotFound(string Message) => base.NotFound(ErrorResult.NotFound(Message));

        /// <summary>404 Not Found: Identifiable was not found</summary>
        /// <typeparam name="E"></typeparam>
        /// <typeparam name="F"></typeparam>
        /// <param name="Identifiable"></param>
        /// <returns></returns>
        [NonAction]
        protected NotFoundObjectResult NotFoundIdentifiable<E,F>(E Identifiable) where E : Identifiable<F> 
            => NotFoundItem(nameof(E),Identifiable.ID);

        /// <summary>404 Not Found: Object was not found</summary>
        /// <param name="ItemName"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [NonAction]
        protected NotFoundObjectResult NotFoundItem(string ItemName, object? ID)
            => base.NotFound(ErrorResult.NotFound($"{ItemName} with ID '{ID}' was not found"));

        /// <summary>418 I'm Teapot : A request you don't wish to fulfill</summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        [NonAction]
        protected IActionResult Teacup(string? Message = null)
            => Message is null
                ? StatusCode(418) 
                : StatusCode(418, Message);

    }
}
