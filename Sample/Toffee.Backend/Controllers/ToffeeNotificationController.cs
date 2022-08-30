using Igtampe.ChopoSessionManager;
using Igtampe.Controllers;
using Igtampe.Toffee.Common;
using Igtampe.Toffee.Data;
using Microsoft.AspNetCore.Mvc;

namespace Igtampe.Toffee.Backend.Controllers {
    
    [Route("API/Notifications")]
    [ApiController]
    public class ToffeeNotificationController : NotificationController<ToffeeContext, Notification, User> {

        public ToffeeNotificationController(ToffeeContext Context)  : base(Context, SessionManager.Manager) {}

        //We have nothing else here
    }
}
