using Igtampe.BasicGraphics;
using Igtampe.Redistributables.Launcher;
using Igtampe.Toffee.Backend.ExceptionHandling;
using Igtampe.Toffee.Data;

namespace Igtampe.Toffee.Backend {
    internal static class Program {
        private static void Main(string[] args) {
            Launcher.Launch<ToffeeContext,ToffeeExceptionHandlingMiddleware>(new() {
                AllowAllCORS = true,
                AlwaysDev = true,
                App = new() {
                    Name = "Toffee",
                    Description = "To-Do For Everyone! A simple demo of the IRED/IDACRA system.",
                    License = "CC0",
                    ProducesXML = false,
                    XMLDocLoc = "",
                    Version = "v1",
                    Graphic = HiColorGraphic.LoadFromResource(Properties.Resources.toffeehc)
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