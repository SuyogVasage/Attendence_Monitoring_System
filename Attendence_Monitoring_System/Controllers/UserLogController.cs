

namespace Attendence_Monitoring_System.Controllers
{
    public class UserLogController : Controller
    {
        private readonly IService<UserLog, int> userLogServ;

        public UserLogController(IService<UserLog, int> userLogServ)
        {
            this.userLogServ = userLogServ;
        }

        public IActionResult Create()
        {
            UserLog userLog = new UserLog();
            return View(userLog);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string Status)
        {
            UserLog userLog = new UserLog();
            //userLog.UserId = HttpContext.Session.GetInt32("UserId");
            userLog.UserId = 1002;
            userLog.Time = DateTime.Now;
            userLog.Status = Status;
            var result = userLogServ.CreateAsync(userLog).Result;
            return View(userLog);
        }

    }
}
