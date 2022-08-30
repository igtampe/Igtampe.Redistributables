using Igtampe.Actions.Exceptions;
using Igtampe.Controllers;
using Igtampe.Controllers.ExceptionHandling;
using Igtampe.Controllers.Exceptions;
using Igtampe.Toffee.Common.Exceptions;


namespace Igtampe.Toffee.Backend.ExceptionHandling {

    /// <summary>Default implementation of the ExceptionHandlingMiddleware with all common redistributable exceptions</summary>
    public class ToffeeExceptionHandlingMiddleware : ExceptionHandlingMiddleware {

        /// <summary>Creates a handling middleware</summary>
        /// <param name="next"></param>
        public ToffeeExceptionHandlingMiddleware(RequestDelegate next) : base(next) {}

        /// <summary>Processes an exception and turns it into an ErrorResult for the EcxeptionHandling Middleware</summary>
        /// <param name="error"></param>
        /// <returns></returns>
        protected override ErrorResult ExceptionToErrorResult(Exception error) 
            => error switch {
                FileTooLargeException or 
                UnacceptableMimeTypeException or
                UsernameAlreadyExistsException or
                TaskReadOnlyException or
                TaskUnassignableException or
                TaskStateTransitionException
                    => ErrorResult.BadRequest(error.Message),
                
                NotTaskAssigneeException or
                NotTaskAssignerException or
                CategoryNotOwnedException
                    => ErrorResult.Forbidden(error.Message),

                CategoryNotFoundException or 
                TaskNotFoundException
                    => ErrorResult.NotFound(error.Message),
                
                CategoryException or 
                TaskException
                    => ErrorResult.ServerError(error.Message),                
                
                _ 
                    => base.ExceptionToErrorResult(error),
            };
        
    }
}
