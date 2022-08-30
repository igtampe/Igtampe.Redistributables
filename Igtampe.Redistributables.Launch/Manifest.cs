namespace Igtampe.Redistributables.Launcher {

    /// <summary>An IDACRA App Manifest File</summary>
    public class Manifest {

        /// <summary>Version of IDACRA used to host this application</summary>
        public string IDACRA_VERSION { get; } = "1.0.0";

        /// <summary>Name of the Application</summary>
        public string Name { get; set; } = "";

        /// <summary>Description of the application</summary>
        public string Description { get; set; } = "";

        /// <summary>License for this application (Either name or link to it)</summary>
        public string License { get; set; } = "";

        /// <summary>Version of the application (IE: V1)</summary>
        public string Version { get; set; } = "V1";

        /// <summary>Details of the author</summary>
        public AuthorDetails Author { get; set; } = new();

    }
}
