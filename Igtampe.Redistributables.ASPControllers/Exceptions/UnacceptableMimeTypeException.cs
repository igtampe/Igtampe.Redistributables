namespace Igtampe.Controllers.Exceptions {

    /// <summary>Exception that's thrown if an HTTP Request has an unacceptable MIME type</summary>
    public class UnacceptableMimeTypeException : Exception {

        /// <summary>List of acceptable MIME Types</summary>
        public string[] AcceptableMIMETypes { get; set; }

        /// <summary>Actual MIME received</summary>
        public string ActualMIME { get; set; }

        /// <summary>Creates an UnacceptableMIMETypeException</summary>
        /// <param name="AcceptableMIMETypes"></param>
        /// <param name="ActualMIME"></param>
        public UnacceptableMimeTypeException(string[] AcceptableMIMETypes, string ActualMIME) {
            this.AcceptableMIMETypes = AcceptableMIMETypes;
            this.ActualMIME = ActualMIME;
        }

        /// <summary>Message of this exception</summary>
        public override string Message => $"File uploaded was of type '{ActualMIME}'. " +
            $"File must be of type or types '{string.Join(", ", AcceptableMIMETypes)}'";

    }
}
