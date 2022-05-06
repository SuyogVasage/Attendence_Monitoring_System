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
            if(Status == "OUT")
            {
                int userId = 1002;
                var res = userLogServ.GetAsync().Result.Where(x => x.UserId == userId);
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
                attendenceLog.UserId = userId;
                attendenceLog.TotalHours = totalHours;
                attendenceLog.Date = DateTime.Parse(currentDate);
                if (dateExist.ToShortDateString() != null)
                {
                    var update = attendenceLogServ.UpdateAsync(Id, attendenceLog).Result;
                }
                else
                {
                    var create = attendenceLogServ.CreateAsync(attendenceLog).Result;
                }
            }
            return View(userLog);
        }

    }
}
