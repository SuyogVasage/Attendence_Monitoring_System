using Newtonsoft.Json;

namespace Attendence_Monitoring_System.Controllers
{
    public class UserLogController : Controller
    {
        private readonly IService<UserLog, int> userLogServ;
        private readonly IService<AttendenceLog, int> attendenceLogServ;

        public UserLogController(IService<UserLog, int> userLogServ, IService<AttendenceLog, int> attendenceLogServ)
        {
            this.userLogServ = userLogServ;
            this.attendenceLogServ = attendenceLogServ;
        }

        public IActionResult Create()
        {
            UserLog userLog = new UserLog();
            int hr = 0, min = 0, sec = 0;
            ViewBag.inOut = calculatTime(out hr, out min, out sec);
            ViewBag.hr = hr;
            ViewBag.min = min;
            ViewBag.sec = sec;
            return View(userLog);
        }

        [HttpPost]
        public IActionResult Create(string Status)
        {
            UserLog userLog = new UserLog();
            userLog.UserId = HttpContext.Session.GetInt32("UserId");
            userLog.Time = DateTime.Now;
            userLog.Status = Status;
            var result = userLogServ.CreateAsync(userLog).Result;
            if(Status == "OUT")
            {
                var res = userLogServ.GetAsync().Result.Where(x => x.UserId == HttpContext.Session.GetInt32("UserId"));
                var res1 = res.Where(x => x.Time.ToShortDateString() == DateTime.Now.ToShortDateString());  
                double totalHours = 0;
                string currentDate = String.Empty;
                foreach (var item in res1)
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
                var dateExist = attendenceLogServ.GetAsync().Result.Where(x => x.Date == DateTime.Parse(currentDate)).Select(x => x.Date).FirstOrDefault();
                var Id = attendenceLogServ.GetAsync().Result.Where(x => x.Date == DateTime.Parse(currentDate)).Select(x => x.Id).FirstOrDefault();
                AttendenceLog attendenceLog = new AttendenceLog();
                attendenceLog.Id = Id;
                attendenceLog.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                attendenceLog.TotalHours = totalHours;
                attendenceLog.Date = DateTime.Parse(currentDate);
                if (dateExist.ToShortDateString() != "01-01-0001")
                {
                    var update = attendenceLogServ.UpdateAsync(Id, attendenceLog).Result;
                }
                else
                {
                    var create = attendenceLogServ.CreateAsync(attendenceLog).Result;
                }
                int hr = 0, min = 0, sec = 0;
                ViewBag.inOut = calculatTime(out hr, out min, out sec);
                ViewBag.hr = hr;
                ViewBag.min = min;
                ViewBag.sec = sec;
            }
            return View(userLog);
        }



        public int calculatTime(out int hr, out int min, out int sec)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.RoleId = HttpContext.Session.GetInt32("RoleId");
            var lastStatus = userLogServ.GetAsync().Result.Where(x => x.UserId == UserId).Select(x => x.Status).LastOrDefault();
            var lastDate = userLogServ.GetAsync().Result.Where(x => x.UserId == UserId).Select(x => x.Time).LastOrDefault().ToShortDateString();
            var res = userLogServ.GetAsync().Result.Where(x => x.UserId == UserId);
            var res1 = res.Where(x => x.Time.ToShortDateString() == DateTime.Now.ToShortDateString()).Select(x => x.Time).FirstOrDefault();
            var onlyTime = res1.ToLongTimeString();
            var currentTime = DateTime.Now.ToLongTimeString();
            TimeSpan duration = DateTime.Parse(currentTime).Subtract(DateTime.Parse(onlyTime));
            hr = Convert.ToInt32(duration.Hours);
            min = Convert.ToInt32(duration.Minutes);
            sec = Convert.ToInt32(duration.Seconds);
            if (lastStatus == "IN" && lastDate == DateTime.Now.ToShortDateString())
            {
                return 1;
            }
            if (lastStatus == "OUT" && lastDate == DateTime.Now.ToShortDateString())
            {
                return 0;
            }
           else
            {
                return 2;
            }
        }

    }
}



