using Microsoft.AspNetCore.Mvc;
using Igtampe.ChopoSessionManager;
using Microsoft.EntityFrameworkCore;
using Igtampe.DBContexts;
using Igtampe.ChopoImageHandling;
using Igtampe.ChopoAuth;
using Igtampe.Actions;

namespace Igtampe.Controllers {

    /// <summary>Controller that handles User operations</summary>
    [Route("API/Images")]
    [ApiController]
    public class ImageController<E,F> : ErrorResultControllerBase where E : DbContext, IImageContext, IUserContext<F> where F : User {

        /// <summary>Configurable maximum size of images accepted by this controller</summary>
        protected virtual int MaxMegabyteSize { get; set; } = 1;
        private readonly ImageAgent<E,F> Agent;

        /// <summary>Creates a User Controller</summary>
        /// <param name="Context"></param>
        /// <param name="Manager">Optional session manager</param>
        public ImageController(E Context, ISessionManager? Manager = null) 
            => Agent = new(Context, Manager ?? SessionManager.Manager);

        /// <summary>Gets an image from the DB</summary>
        /// <param name="ID">ID of the image to retrieve</param>
        /// <returns></returns>
        [HttpGet("{ID}")]
        public async Task<IActionResult> GetImage(Guid ID) {
            Image I = await Agent.GetImage(ID);
            return File(I.Data, I.Type);
        }

        /// <summary>Gets an image from the DB</summary>
        /// <param name="ID">ID of the image to retrieve</param>
        /// <returns></returns>
        [HttpGet("{ID}/Info")]
        public async Task<IActionResult> GetImageInfo(Guid ID) 
            => Ok(await Agent.GetImage(ID));

        /// <summary>Uploads an Image to the DB.</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <returns></returns>
        // POST api/Images
        [HttpPost]
        public async Task<IActionResult> UploadImage([FromHeader] Guid SessionID) {
            Image I = await ControllerUtils.GetImageFromRequest(Request, MaxMegabyteSize);
            await Agent.CreateImage(SessionID, I);
            return Ok(I);

        }
    }
}
