namespace Igtampe.Redistributables.Launcher {
    /// <summary>Options for the IRED/IDACRA Launcher</summary>
    public class LauncherOptions {

        /// <summary>Details of the app</summary>
        public AppDetails App { get; set; } = new();

        /// <summary>Details of the author</summary>
        public AuthorDetails Author { get; set; } = new();

        /// <summary>Whether or not to allow all CORS </summary>
        public bool AllowAllCORS { get; set; } = true;

        /// <summary>Whether to always behave like a Dev or not. Setting this to true will expose the Dev exception page and make the swagger always available</summary>
        public bool AlwaysDev { get; set; } = false;

        /// <summary>Converts Launch Options to a manifest object</summary>
        /// <returns></returns>
        public Manifest ToManifest() => new() {
                Author = Author,
                Description = App.Description,
                License=App.License,
                Name=App.Name,
                Version=App.Version
        };

    }
}
