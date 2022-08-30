using Igtampe.Redistributables.Launcher;

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