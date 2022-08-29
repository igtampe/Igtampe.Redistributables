namespace Igtampe.DBContexts.Exceptions {
    /// <summary>Exception thrown if the DBURL is not parsable</summary>
    public class DBURLUnparsableException : Exception{

        /// <summary>DBURL that was unparsable</summary>
        public string DBURL { get; private set; }

        /// <summary>Creates a DBURL Unparsable Exception</summary>
        /// <param name="DBURL"></param>
        public DBURLUnparsableException(string DBURL) => this.DBURL = DBURL;

        /// <summary>Message for this exception</summary>
        public override string Message => $"Database URL \"{DBURL}\" was unparsable!";
    }
}
