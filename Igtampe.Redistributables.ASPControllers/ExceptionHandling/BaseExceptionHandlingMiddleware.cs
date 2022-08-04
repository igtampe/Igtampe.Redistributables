using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace Igtampe.Controllers.ExceptionHandling {
    /// <summary>Basic extensible exception Handler middleware</summary>
    public abstract class BaseExceptionHandlingMiddleware {

        private readonly RequestDelegate _next;

        /// <summary>Creates an Exception Handling Middleware</summary>
        /// <param name="next"></param>
        protected BaseExceptionHandlingMiddleware(RequestDelegate next) => _next = next;

        /// <summary>Invokes</summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context) {
            try {
                await _next(context);
            } catch (Exception error) {
                var response = context.Response;
                response.ContentType = "application/json";

                var ER = ExceptionToErrorResult(error);
                response.StatusCode = ER.Code;

                var result = JsonSerializer.Serialize(ER);
                await response.WriteAsync(result);
            }
        }

        /// <summary>Processes your custom exceptions to an error result</summary>
        /// <param name="error"></param>
        /// <returns></returns>
        protected virtual ErrorResult ExceptionToErrorResult(Exception error) =>
#if DEBUG
            ErrorResult.ServerError($"{error.GetType().FullName}: {error.Message}\n\n{error.StackTrace}");
#else
            return ErrorResult.ServerError("An unknown server error occurred");
#endif
    }
}
