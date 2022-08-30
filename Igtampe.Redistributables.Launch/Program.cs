namespace Igtampe.Redistributables.Launcher {

    /// <summary>Main Program entry-point for the demo</summary>
    public static class Program {

        /// <summary>Main application demo</summary>
        /// <param name="args"></param>
        public static void Main(string[] args) {

            Launcher.Launch(new() {
                AllowAllCORS = true,
                AlwaysDev = true,
                App = new() {
                    Name = "IDACRA Test App",
                    Description = "This is a test to ensure the launcher actually works",
                    License = "CC0",
                    ProducesXML = false,
                    XMLDocLoc = "",
                    Version = "v1"
                },
                Author = new() {
                    Name = "Chopo",
                    Email = "me@igtampe.com",
                    Url = "https://www.igtampe.com"
                }
            }, args);

        }

    }

}