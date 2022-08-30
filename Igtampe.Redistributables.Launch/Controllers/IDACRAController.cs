using Microsoft.AspNetCore.Mvc;

namespace Igtampe.Redistributables.Launcher.Controllers {

    /// <summary>Test controller always present to show IDACRA is running</summary>
    [Route("IDACRA")]
    [ApiController]
    public class IDACRAController : ControllerBase {

        /// <summary>Info on IDACRA</summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Info() => Ok(Launcher.Options?.ToManifest());

    }
}
