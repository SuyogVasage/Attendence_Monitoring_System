
namespace Attendence_Monitoring_System.Controllers
{
    public class AttendenceLogController : Controller
    {
        private readonly IService<AttendenceLog, int> attendenceLogServ;
        private readonly IService<UserLog, int> userLogserv;
        private readonly IDataAccess iDataAccess;

        public AttendenceLogController(IService<AttendenceLog, int> attendenceLogServ, 
            IService<UserLog, int> userLogserv, IDataAccess iDataAccess)
        {
            this.attendenceLogServ = attendenceLogServ;
            this.userLogserv = userLogserv;
            this.iDataAccess = iDataAccess;
        }

        //User Can see his Data only
        public IActionResult Get()
        {
            //Calculating Realtime Attendance when Check IN
            iDataAccess.CalculateRealTime(HttpContext.Session.GetInt32("UserId"), 1);
            var res = attendenceLogServ.GetAsync().Result.Where(x=>x.UserId == HttpContext.Session.GetInt32("UserId"));
            return View(res);
        }

        //Admin can see Employees Details of their choice 
        public IActionResult GetForAdmin()
        {
            //Calculating Realtime Attendance Check IN for Admin Selected User
            iDataAccess.CalculateRealTime(HttpContext.Session.GetInt32("UserId1"), 0);
            var res = attendenceLogServ.GetAsync().Result.Where(x => x.UserId == HttpContext.Session.GetInt32("UserId1"));
            return View(res);
        }

        //All IN's and OUT's for that Day
        public IActionResult Details(int Id)
        {
            int UserId = attendenceLogServ.GetAsync().Result.Where(x => x.Id == Id).Select(x => x.UserId).FirstOrDefault();
            var Date = attendenceLogServ.GetAsync().Result.Where(x => x.Id == Id).Select(x => x.Date.ToShortDateString()).FirstOrDefault();
            var userLogs = userLogserv.GetAsync().Result.Where(x => x.UserId == UserId);
            var userLog = userLogs.Where(x=> x.Time.ToShortDateString() == Date);
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            return View(userLog);
        }


        //Editing After Regularization
        public IActionResult Edit()
        {
            Regularization regularization= HttpContext.Session.GetObject<Regularization>("UpdateData");
            AttendenceLog attendenceLog= new AttendenceLog();
            attendenceLog.TotalHours = regularization.TotalHours;
            var regularizationDate = regularization.InTime.ToShortDateString();
            var dateList = attendenceLogServ.GetAsync().Result.Where(x => x.UserId == regularization.UserId).Select(x => x.Date.ToShortDateString());
            var res = dateList.Where(x=>x.Equals(regularizationDate)).FirstOrDefault();
            attendenceLog.Date = DateTime.Parse(res);
            attendenceLog.UserId = Convert.ToInt32(regularization.UserId);
            attendenceLog.Id = attendenceLogServ.GetAsync().Result.Where(x=>x.Date == attendenceLog.Date).Select(x=>x.Id).FirstOrDefault();
            var result = attendenceLogServ.UpdateAsync(attendenceLog.Id, attendenceLog);
            return RedirectToAction("Get", "Regularization");
        }

    }
}
