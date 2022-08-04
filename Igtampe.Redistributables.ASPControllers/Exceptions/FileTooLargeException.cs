namespace Igtampe.Controllers.Exceptions {
    /// <summary>Exception that occurs if a user tries to upload a file that is larger than the established maximum allowed size</summary>
    public class FileTooLargeException : Exception {

        /// <summary>Shortcut function to turn Bytes into Megabytes</summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static double BytesToMB(int bytes) => bytes / 1024.0 / 1024.0;

        /// <summary>Maximum size of the file in bytes</summary>
        public int MaxSizeBytes { get; set; }

        /// <summary>Actual size of the file in bytes</summary>
        public int ActualSizeBytes { get; set; }

        /// <summary>Actual size of the file in Megabytes</summary>
        public double ActualSizeMegabytes => BytesToMB(ActualSizeBytes);

        /// <summary>Maximum size of the file in Megabytes</summary>
        public double MaxSizeMegabytes => BytesToMB(MaxSizeBytes);

        /// <summary>Creates a FileTooLargeException</summary>
        /// <param name="MaxSize"></param>
        /// <param name="ActualSize"></param>
        public FileTooLargeException(int MaxSize, int ActualSize) {
            ActualSizeBytes = ActualSize;
            MaxSizeBytes= MaxSize;
        }
        
        /// <summary>Message for this exception</summary>
        public override string Message => $"File to upload was too large! Maximum is {MaxSizeMegabytes:n2}MB but was {ActualSizeMegabytes:n2}MB";

    }
}
