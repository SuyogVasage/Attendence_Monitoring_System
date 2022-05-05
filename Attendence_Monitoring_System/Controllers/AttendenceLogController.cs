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
        public IActionResult Create()
        {
            int userId = 1002;
            var res = userLogserv.GetAsync().Result.Where(x=>x.UserId == userId);
            var res1 = res.Where(x => x.Time.ToShortDateString() == DateTime.Now.ToShortDateString());
            double totalHours = 0;
            string currentDate = String.Empty;
            foreach(var item in res1)
            {
                if (item.Status == "IN")
                {
                    ViewBag.time = item.Time;
                }
                else
                {
                    var inDateTime = ViewBag.time;
                    var outDateTime = item.Time;
                    currentDate = inDateTime.ToShortDateString();
                    totalHours = (outDateTime - inDateTime).TotalHours;
                    totalHours += totalHours;
                }
            }
            AttendenceLog attendenceLog = new AttendenceLog();
            attendenceLog.UserId = userId;
            attendenceLog.TotalHours = totalHours;
            attendenceLog.Date = DateTime.Parse(currentDate);
            var result = attendenceLogServ.CreateAsync(attendenceLog).Result;
            return RedirectToAction("Get");
        }

        public IActionResult Get()
        {
            var res = attendenceLogServ.GetAsync().Result.Where(x=>x.UserId == 1002);
            return View(res);
        }

        public IActionResult Details(int Id)
        {
            int UserId = attendenceLogServ.GetAsync().Result.Where(x => x.Id == Id).Select(x => x.UserId).FirstOrDefault();
            var Date = attendenceLogServ.GetAsync().Result.Where(x => x.Id == Id).Select(x => x.Date.ToShortDateString()).FirstOrDefault();
            var res = userLogserv.GetAsync().Result.Where(x => x.UserId == UserId);
            var res1 = res.Where(x=> x.Time.ToShortDateString() == Date);
            return View(res1);
        }

    }
}
