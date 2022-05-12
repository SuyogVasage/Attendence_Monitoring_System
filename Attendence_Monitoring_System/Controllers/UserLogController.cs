using Newtonsoft.Json;

namespace Attendence_Monitoring_System.Controllers
{
    public class UserLogController : Controller
    {
        private readonly IService<UserLog, int> userLogServ;
        private readonly IService<AttendenceLog, int> attendenceLogServ;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Attendence_Monitoring_SystemContext ctx;
        public DataAccess dataAccess;

        public UserLogController(IService<UserLog, int> userLogServ, 
            IService<AttendenceLog, int> attendenceLogServ, 
            Attendence_Monitoring_SystemContext ctx, IHttpContextAccessor _httpContextAccessor)
        {
            this.userLogServ = userLogServ;
            this.attendenceLogServ = attendenceLogServ;
            this._httpContextAccessor = _httpContextAccessor;
            this.ctx = ctx;
            dataAccess = new DataAccess(_httpContextAccessor, ctx);
        }

        public IActionResult Create(int UserID)
        {
            UserLog userLog = new UserLog();
            //Calculating Time for Timer on View
            CalulateTime();
            return View(userLog);
        }

        [HttpPost]
        public IActionResult Create(string Status)
        {
            UserLog userLog = new UserLog();
            //Calculating Attendence by Status
            userLog = dataAccess.calculateAttendance(Status);
            //Calculating Time for Timer on View
            CalulateTime();
            return View(userLog);
        }

        public void CalulateTime()
        {
            int hr = 0, min = 0, sec = 0;
            int? RoleId = 0;
            //Method to get Hours, Minutes, Seconds for Timer
            ViewBag.inOut = dataAccess.calculatTime(out hr, out min, out sec, out RoleId);
            ViewBag.hr = hr;
            ViewBag.min = min;
            ViewBag.sec = sec;
            ViewBag.RoleId = RoleId;
        }

    }
}



