namespace Igtampe.DBContexts.Exceptions {
    /// <summary>Exception thrown if the DBURL is not parsable</summary>
    public class DBURLNotFoundException : Exception {

        /// <summary>Message for this exception</summary>
        public override string Message => $"Could not find DBURL on Environment Variable or at \"{Path.Combine(Directory.GetCurrentDirectory(),"DBURL.txt")}\"";
    }
}
