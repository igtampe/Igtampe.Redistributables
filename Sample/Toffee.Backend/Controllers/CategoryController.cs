using Igtampe.ChopoImageHandling;
using Igtampe.ChopoSessionManager;
using Igtampe.Controllers;
using Igtampe.Toffee.Actions;
using Igtampe.Toffee.Backend.Requests.Category;
using Igtampe.Toffee.Data;
using Microsoft.AspNetCore.Mvc;

namespace Igtampe.Toffee.Backend.Controllers {

    [Route("API/Categories")]
    [ApiController]
    public class CategoryController : ControllerBase {

        private readonly CategoryAgent Agent;

        public CategoryController(ToffeeContext Context)
            => Agent = new(Context, SessionManager.Manager);

        [HttpGet]
        public async Task<IActionResult> Categories([FromHeader] Guid? SessionID) => Ok(
            SessionID is null || SessionID == Guid.Empty ? await Agent.GetAllCategories() : await Agent.GetMyCategories(SessionID));
        
        [HttpGet("{ID}")]
        public async Task<IActionResult> Category([FromRoute] Guid ID) => Ok(await Agent.GetCategory(ID));

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromHeader] Guid? SessionID, [FromBody] CategoryRequest Request)
            => Ok(await Agent.CreateCategory(SessionID, Request.Name, Request.Description, Request.Color));

        [HttpPut("{ID}")]
        public async Task<IActionResult> UpdateCategory([FromHeader] Guid? SessionID, [FromRoute] Guid ID, [FromBody] CategoryRequest Request)
                    => Ok(await Agent.UpdateCategory(SessionID, ID, Request.Name, Request.Description, Request.Color));

        [HttpPut("{ID}/image")]
        public async Task<IActionResult> UpdateCategoryIcon([FromHeader] Guid? SessionID, [FromRoute] Guid ID) {
            Image I = await ControllerUtils.GetImageFromRequest(Request);
            return Ok(await Agent.UpdateCategoryIcon(SessionID, ID, I));
        }
    }
}
