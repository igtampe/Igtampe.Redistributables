using Igtampe.ChopoSessionManager;
using Igtampe.Controllers;
using Igtampe.Toffee.Common;
using Igtampe.Toffee.Data;
using Microsoft.AspNetCore.Mvc;

namespace Igtampe.Toffee.Backend.Controllers {

    [Route("API/Images")]
    [ApiController]
    public class ToffeeImageController : ImageController<ToffeeContext, User> {
        public ToffeeImageController(ToffeeContext Context) : base(Context, SessionManager.Manager) {}

        //We also have nothing else here
    }
}
