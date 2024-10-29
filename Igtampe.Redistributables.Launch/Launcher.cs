using Igtampe.BasicRender;
using Igtampe.BasicGraphics;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Igtampe.Redistributables.Launcher {
    /// <summary>The IRED/IDACRA Launcher</summary>
    public static class Launcher {

        /// <summary>Whether or not the Launcher is launched</summary>
        public static bool IsLaunched { get; private set; } = false;

        /// <summary>Options the launcher was launched with</summary>
        public static LauncherOptions? Options { get; private set; }

        private sealed class EmptyContext : DbContext { }
        private sealed class EmptyMiddleware {

            private readonly RequestDelegate _next;
            public EmptyMiddleware(RequestDelegate next) => _next = next;
            public async Task InvokeAsync(HttpContext httpContext) => await _next(httpContext);

        }

        /// <summary>Launches the application with specified options and no DB Context configured or middleware</summary>
        /// <param name="LaunchOptions"></param>
        /// <param name="args"></param>
        public static void Launch(LauncherOptions LaunchOptions, string[]? args = null) => Launch<EmptyContext>(LaunchOptions, args);

        /// <summary>Launches the application with specified options and provided master DB Context, but no middleware</summary>
        /// <typeparam name="E">Master DB Context</typeparam>
        /// <param name="LaunchOptions"></param>
        /// <param name="args"></param>
        public static void Launch<E>(LauncherOptions LaunchOptions, string[]? args = null)
            where E : DbContext
            => Launch<E,EmptyMiddleware>(LaunchOptions, args);

        /// <summary>Launches the application with specified options and provided master DB Context and provided middleware</summary>
        /// <typeparam name="E">Master DB Context</typeparam>
        /// <typeparam name="F">Middleware</typeparam>
        /// <param name="LaunchOptions"></param>
        /// <param name="args"></param>
        public static void Launch<E,F>(LauncherOptions LaunchOptions, string[]? args = null) where E : DbContext {

            Options = LaunchOptions;

            //Logo
            try {DrawLogo(LaunchOptions);} catch (Exception) {} //Try catch just in case we're not in a console

            var builder = WebApplication.CreateBuilder(args ?? Array.Empty<string>());
            var CORS = "CORS";

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(o => {
                o.SwaggerDoc(LaunchOptions.App.Version, new OpenApiInfo {
                    Version = LaunchOptions.App.Version, Title = LaunchOptions.App.Name,
                    Description = LaunchOptions.App.Description,

                    Contact = LaunchOptions.Author.ToOAC(),

                    License = LaunchOptions.App.ToOAL()
                });
                
            });
            
            builder.Services.AddDbContext<E>();

            builder.Services.AddCors(o => {
                o.AddPolicy(name: CORS,
                builder => {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowAnyOrigin();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() || LaunchOptions.AlwaysDev) {
                app.UseSwagger();
                app.UseSwaggerUI(options=>options.SwaggerEndpoint($"/swagger/{LaunchOptions.App.Version}/swagger.json",LaunchOptions.App.Name));
                app.UseDeveloperExceptionPage();
            }

            if (LaunchOptions.AllowAllCORS) { app.UseCors(CORS); }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.UseMiddleware<F>();

            app.MapControllers();

            app.Run();

            //if somehow this fails
            IsLaunched = false;

        }

        private static void DrawLogo(LauncherOptions Options) {

            //Clear
            Console.Clear();

            //Draw the logo
            var Logo = Options.App.Graphic ?? BasicGraphic.LoadFromResource(Properties.Resources.IDACRA);

            int TextLeftOffset = 2 + Logo.GetWidth() + 2;
            int TextTopOffset = 1 + (Convert.ToInt32(Math.Floor(Logo.GetHeight() / 2.0))-1);
            int BarTopOffset = 1 + Logo.GetHeight();

            Manifest M = Options.ToManifest();
            string L1 = $"{M.Name} ({M.Version})";
            string L2 = $"Powered by IDACRA ({M.idacra_version})";

            int TextMaxLength = Math.Max(L1.Length, L2.Length);

            bool ForceOneRow = false;

            if (Console.WindowWidth < Logo.GetWidth() + 6 + TextMaxLength) {
                TextLeftOffset = 0;
                TextTopOffset = 0;
                BarTopOffset = 3;
                ForceOneRow = true;
            } else {
                Logo.Draw(2, 1);
            }

            var Bar = PrepBar(ForceOneRow);

            Draw.Sprite(L1, Console.BackgroundColor, Console.ForegroundColor, TextLeftOffset, TextTopOffset);
            Draw.Sprite(L2, Console.BackgroundColor, Console.ForegroundColor, TextLeftOffset, TextTopOffset + 1);

            Bar.Draw(0, BarTopOffset);

            Console.WriteLine();
            Console.WriteLine();

        }

        private static readonly string[] BarColors = { 
            "500", "D00", "CD1", "440","6C1","6E0"
        };

        private static Graphic PrepBar(bool ForceOneRow) {
            //Draw the bars
            //Let's generate a HiColor image
            //Divide the console width by 6
            double ColorWidthD = Console.WindowWidth / (BarColors.Length * 1.0);
            int ColorWidth = Convert.ToInt32(Math.Floor(ColorWidthD));

            //If we cannot fit the bars, return
            if (ColorWidth < 1) { return new BasicGraphic(Array.Empty<string>(),"Empty graphic"); }

            //Build a row
            StringBuilder BarHC = new(BarColors[0]) ; //Always start with color 0

            for (int i = 1; i < ColorWidth; i++) {
                BarHC.Append($"-{BarColors[0]}");
            }

            for (int i = 1; i < BarColors.Length; i++) {
                for (int j = 0; j < ColorWidth; j++) {
                    BarHC.Append($"-{BarColors[i]}");
                }
            }

            //Prepare the HC Contents
            int RowCount = Console.WindowWidth < 60 || ForceOneRow ? 1 : 2;
            List<string> HCContents = new(); 
            for (int i = 0; i < RowCount; i++) {
                HCContents.Add(BarHC.ToString());
            }

            HiColorGraphic Bar = new(HCContents.ToArray(), "HiColor Bar");

            return Bar;
        }
    }
}
