using Microsoft.AspNetCore.Mvc;

namespace Attendence_Monitoring_System.Controllers
{
    public class AttendenceLogController : Controller
    {
        private readonly IService<AttendenceLog, int> attendenceLogServ;
        private readonly IService<UserLog, int> userLogserv;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Attendence_Monitoring_SystemContext ctx;
        public DataAccess dataAccess;

        public AttendenceLogController(IService<AttendenceLog, int> attendenceLogServ, 
            IService<UserLog, int> userLogserv, IHttpContextAccessor _httpContextAccessor,
            Attendence_Monitoring_SystemContext ctx)
        {
            this.attendenceLogServ = attendenceLogServ;
            this.userLogserv = userLogserv;
            this._httpContextAccessor = _httpContextAccessor;
            this.ctx = ctx;
            dataAccess = new DataAccess(_httpContextAccessor, ctx);
        }

        //User Can see his Data only
        public IActionResult Get()
        {
            //Calculating Realtime Attendance when Check IN
            dataAccess.CalculateRealTime(HttpContext.Session.GetInt32("UserId"));
            var res = attendenceLogServ.GetAsync().Result.Where(x=>x.UserId == HttpContext.Session.GetInt32("UserId"));
            return View(res);
        }

        //Admin can see Employees Details of their choice 
        public IActionResult GetForAdmin()
        {
            //Calculating Realtime Attendance Check IN for Admin Selected User
            dataAccess.CalculateRealTime(HttpContext.Session.GetInt32("UserId1"));
            var res = attendenceLogServ.GetAsync().Result.Where(x => x.UserId == HttpContext.Session.GetInt32("UserId1"));
            return View(res);
        }

        //All IN's and OUT's for that Day
        public IActionResult Details(int Id)
        {
            int UserId = attendenceLogServ.GetAsync().Result.Where(x => x.Id == Id).Select(x => x.UserId).FirstOrDefault();
            var Date = attendenceLogServ.GetAsync().Result.Where(x => x.Id == Id).Select(x => x.Date.ToShortDateString()).FirstOrDefault();
            var res = userLogserv.GetAsync().Result.Where(x => x.UserId == UserId);
            var res1 = res.Where(x=> x.Time.ToShortDateString() == Date);
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            return View(res1);
        }


        //Editing After Regularization
        public IActionResult Edit()
        {
            Regularization regularization= HttpContext.Session.GetObject<Regularization>("UpdateData");
            AttendenceLog attendenceLog= new AttendenceLog();
            attendenceLog.TotalHours = regularization.TotalHours;
            var DateR = regularization.InTime.ToShortDateString();
            var DateA = attendenceLogServ.GetAsync().Result.Where(x => x.UserId == regularization.UserId).Select(x => x.Date.ToShortDateString());
            var res = DateA.Where(x=>x.Equals(DateR)).FirstOrDefault();
            attendenceLog.Date = DateTime.Parse(res);
            var userID = regularization.UserId;
            attendenceLog.UserId = Convert.ToInt32(userID);
            attendenceLog.Id = attendenceLogServ.GetAsync().Result.Where(x=>x.Date == attendenceLog.Date).Select(x=>x.Id).FirstOrDefault();
            var result = attendenceLogServ.UpdateAsync(attendenceLog.Id, attendenceLog);
            return RedirectToAction("Get", "Regularization");
        }

    }
}
