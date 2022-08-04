namespace Igtampe.Controllers.Exceptions {
    /// <summary>
    /// Exception that's thrown when a Teapot is told through HTTP to make Coffee (which it clearly cannot)<br/><br/>
    /// 
    /// Thrown more commonly in circumstances when a server <i>could</i> fulfill the request, but is unwilling or refuses to do so.
    /// </summary>
    public class TeapotException : Exception {

        private string InternalMessage { get; set; } = "I'm a Teapot!";

        /// <summary>Creates a TeapotException</summary>
        public TeapotException() {}

        /// <summary>Creates a TeapotException with a custom message</summary>
        /// <param name="Message"></param>
        public TeapotException(string Message) => InternalMessage = Message;

        /// <summary>Message for this </summary>
        public override string Message => InternalMessage;

    }
}
