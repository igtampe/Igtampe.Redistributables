using Igtampe.Actions.Exceptions;
using Igtampe.ChopoAuth.Exceptions;
using Igtampe.ChopoImageHandling.Exceptions;
using Igtampe.ChopoSessionManager.Exceptions;
using Igtampe.Controllers.Exceptions;
using Igtampe.Notifier.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Igtampe.Controllers.ExceptionHandling {

    /// <summary>Default implementation of the ExceptionHandlingMiddleware with all common redistributable exceptions</summary>
    public class ExceptionHandlingMiddleware : BaseExceptionHandlingMiddleware {

        /// <summary>Creates a handling middleware</summary>
        /// <param name="next"></param>
        public ExceptionHandlingMiddleware(RequestDelegate next) : base(next) {}

        /// <summary>Processes an exception and turns it into an ErrorResult for the EcxeptionHandling Middleware</summary>
        /// <param name="error"></param>
        /// <returns></returns>
        protected override ErrorResult ExceptionToErrorResult(Exception error) 
            => error switch {
                PasswordIncorrectException or 
                FileTooLargeException or 
                UnacceptableMimeTypeException or
                UsernameAlreadyExistsException
                    => ErrorResult.BadRequest(error.Message),
                
                UserRolesException or
                SelfAdminException
                    => ErrorResult.Forbidden(error.Message),

                SessionNotFoundException
                    => ErrorResult.Reusable.InvalidSession,

                UserNotFoundException or 
                ImageNotFoundException or 
                NotificationNotFoundException 
                    => ErrorResult.NotFound(error.Message),
                
                UserException or 
                ImageException or 
                SessionException or 
                NotificationException 
                    => ErrorResult.ServerError(error.Message),
                
                TeapotException
                    => ErrorResult.Teapot(error.Message),

                _ 
                    => base.ExceptionToErrorResult(error),
            };
        
    }
}
