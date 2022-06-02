
namespace Attendence_Monitoring_System.Controllers
{
    public class UserLogController : Controller
    {
        private readonly IService<UserLog, int> userLogServ;
        private readonly IService<AttendenceLog, int> attendenceLogServ;
        private readonly IDataAccess iDataAccess;
        
        public UserLogController(IService<UserLog, int> userLogServ, IDataAccess iDataAccess,
            IService<AttendenceLog, int> attendenceLogServ)
        {
            this.userLogServ = userLogServ;
            this.attendenceLogServ = attendenceLogServ;
            this.iDataAccess = iDataAccess;
        }

        public IActionResult Create()
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
            userLog = iDataAccess.calculateAttendance(Status);
            //Calculating Time for Timer on View
            CalulateTime();
            return View(userLog);
        }

        public void CalulateTime()
        {
            int hr = 0, min = 0, sec = 0;
            int? RoleId = 0;
            //Method to get Hours, Minutes, Seconds for Timer
            ViewBag.inOut = iDataAccess.calculatTime(out hr, out min, out sec, out RoleId);
            ViewBag.hr = hr;
            ViewBag.min = min;
            ViewBag.sec = sec;
            ViewBag.RoleId = RoleId;
        }

    }
}



