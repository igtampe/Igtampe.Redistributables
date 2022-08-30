using Microsoft.OpenApi.Models;

namespace Igtampe.Redistributables.Launcher {

    /// <summary>Details of an application</summary>
    public class AppDetails {

        /// <summary>Name of the application</summary>
        public string Name { get; set; } = "MyApp";

        /// <summary>Internal description</summary>
        private string desc = "Your new app!";

        /// <summary>Description of the application</summary>
        public string Description {
            get => $"{desc}{Environment.NewLine}{Environment.NewLine}Launched using the IRED/IDACRA Launcher";
            set => desc = value;
        }

        /// <summary>License for this application (Either name or link to it)</summary>
        public string License { get; set; } = "";

        /// <summary>Version of the application (IE: V1)</summary>
        public string Version { get; set; } = "V1";

        /// <summary>Whether or not this application produces XML Documentation</summary>
        public bool ProducesXML { get; set; } = false;

        /// <summary>Where the XML documentation is located (set <see cref="ProducesXML"/> to true to use this)</summary>
        public string XMLDocLoc { get; set; } = "";

        /// <summary>Image graphic for the application to appear on console</summary>
        public BasicGraphics.Graphic? Graphic { get; set; } = null;

        /// <summary>Converts the licesne data to OAL</summary>
        /// <returns></returns>
        public OpenApiLicense ToOAL() => License.ToLower().StartsWith("http") ? new() { Url = new(License) } : new() { Name=License };

    }
}
