using Microsoft.AspNetCore.Mvc;

namespace Attendence_Monitoring_System.Controllers
{
    public class AttendenceLogController : Controller
    {
        private readonly IService<AttendenceLog, int> attendenceLogServ;
        private readonly IService<UserLog, int> userLogserv;
        public AttendenceLogController(IService<AttendenceLog, int> attendenceLogServ, IService<UserLog, int> userLogserv)
        {
            this.attendenceLogServ = attendenceLogServ;
            this.userLogserv = userLogserv;
        }
        public IActionResult Get()
        {
            var res = attendenceLogServ.GetAsync().Result.Where(x=>x.UserId == HttpContext.Session.GetInt32("UserId"));
            return View(res);
        }
        public IActionResult GetForAdmin()
        {
            var res = attendenceLogServ.GetAsync().Result.Where(x => x.UserId == HttpContext.Session.GetInt32("UserId1"));
            return View(res);
        }

        public IActionResult Details(int Id)
        {
            int UserId = attendenceLogServ.GetAsync().Result.Where(x => x.Id == Id).Select(x => x.UserId).FirstOrDefault();
            var Date = attendenceLogServ.GetAsync().Result.Where(x => x.Id == Id).Select(x => x.Date.ToShortDateString()).FirstOrDefault();
            var res = userLogserv.GetAsync().Result.Where(x => x.UserId == UserId);
            var res1 = res.Where(x=> x.Time.ToShortDateString() == Date);
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            return View(res1);
        }

    }
}
