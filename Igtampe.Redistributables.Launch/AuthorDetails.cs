using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igtampe.Redistributables.Launcher {

    /// <summary>Details about the author of an application. Shortcut for OpenAPIContact</summary>
    public class AuthorDetails {

        /// <summary>Author's name</summary>
        public string Name { get; set; } = "";

        /// <summary>Author's Email</summary>
        public string Email { get; set; } = "";

        /// <summary>Author's website</summary>
        public string Url { get; set; } = "";

        /// <summary>Converts this details to an OpenAPI Contact</summary>
        /// <returns></returns>
        public OpenApiContact ToOAC() => new() {
            Email = Email, Name = Name, Url = new(Url)
        };

    }
}
