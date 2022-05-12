using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Attendence_Monitoring_System.Services
{
    public class DataAccess
    {
        private readonly Attendence_Monitoring_SystemContext ctx;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DataAccess(IHttpContextAccessor _httpContextAccessor, Attendence_Monitoring_SystemContext ctx)
        {
            this._httpContextAccessor = _httpContextAccessor;
            this.ctx = ctx;
        }

        public EmployeeList EmpController(string SearchOption, string SearchString)
        {
            IEnumerable<UserDetail> res1 = new List<UserDetail>();
            switch (SearchOption)
            {
                case "Name":
                    int UserId = ctx.UserDetails.ToList().Where(e => e.Value.Contains(SearchString)).Select(x => x.UserId).FirstOrDefault();
                    res1 = ctx.UserDetails.ToList().Where(e => e.UserId == UserId);
                    break;
                case "EmpId":
                    if (!System.Text.RegularExpressions.Regex.IsMatch(SearchString, "^[0-9]*$"))
                    {
                        return null;
                    }
                    res1 = ctx.UserDetails.ToList().Where(e => e.UserId == Convert.ToInt32(SearchString));
                    break;
            }

            int UserId1 = res1.Select(x => x.UserId).FirstOrDefault();
            _httpContextAccessor.HttpContext.Session.SetInt32("UserId1", UserId1);
            EmployeeList empList = new EmployeeList();
            empList.users = res1;
            empList.searchOption = SearchOption;
            empList.searchString = SearchString;
            return empList;
        }


        public int calculatTime(out int hr, out int min, out int sec, out int? RoleId)
        {
            int? UserId = _httpContextAccessor.HttpContext.Session.GetInt32("UserId");
            RoleId =_httpContextAccessor.HttpContext.Session.GetInt32("RoleId");

            var lastDate = ctx.UserLogs.ToList().Where(x => x.UserId == UserId).Select(x => x.Time).LastOrDefault().ToShortDateString();
            if(lastDate != DateTime.Now.ToShortDateString())
            {
                hr = 0;
                min= 0;
                sec = 0; 
                return 2;
            }

            var res = ctx.UserLogs.ToList().Where(x => x.UserId == _httpContextAccessor.HttpContext.Session.GetInt32("UserId"));
            var res1 = res.Where(x => x.Time.ToShortDateString() == DateTime.Now.ToShortDateString());
            TimeSpan middleTime;
            TimeSpan TotalTime = TimeSpan.Zero;
            string lastStatus = String.Empty;
            foreach (var item in res1)
            {
                if (item.Status == "IN")
                {
                    _httpContextAccessor.HttpContext.Session.SetString("TempTime", item.Time.ToString());
                    lastStatus = "IN";
                }
                else
                {
                    var inDateTime = DateTime.Parse(_httpContextAccessor.HttpContext.Session.GetString("TempTime"));
                    var outDateTime = item.Time;
                    middleTime = outDateTime.Subtract(inDateTime);
                    TotalTime = TotalTime + middleTime;
                    lastStatus = "OUT";
                }
            }
            
            if(lastStatus == "IN")
            {
                var res2 = res1.Where(x => x.Status == "IN").Select(x => x.Time).LastOrDefault();
                var lastInTime = res1.Select(x => x.Time).LastOrDefault();
                TimeSpan tt = DateTime.Now.Subtract(lastInTime);
                string ttt = $"{tt.Hours}:{tt.Minutes}:{tt.Seconds}";
                TotalTime = TotalTime + tt;
            }
            hr = Convert.ToInt32(TotalTime.Hours);
            min = Convert.ToInt32(TotalTime.Minutes);
            sec = Convert.ToInt32(TotalTime.Seconds);
            if (lastStatus == "IN")
            {
                return 1;
            }
            if (lastStatus == "OUT")
            {
                return 0;
            }
            else
            {
                return 2;
            }
        }

        public UserLog calculateAttendance(string Status)
        {
            UserLog userLog = new UserLog();
            userLog.UserId = _httpContextAccessor.HttpContext.Session.GetInt32("UserId");
            userLog.Time = DateTime.Now;
            userLog.Status = Status;
            ctx.UserLogs.Add(userLog);
            ctx.SaveChanges();

            string lastStatus = String.Empty;
            var res = ctx.UserLogs.ToList().Where(x => x.UserId == _httpContextAccessor.HttpContext.Session.GetInt32("UserId"));
            var res1 = res.Where(x => x.Time.ToShortDateString() == DateTime.Now.ToShortDateString());
            
            TimeSpan totalHours = TimeSpan.Zero;
            string currentDate = String.Empty;
            foreach (var item in res1)
            {
                if (item.Status == "IN")
                {
                    _httpContextAccessor.HttpContext.Session.SetString("TempTime", item.Time.ToString());
                    lastStatus = "IN";
                    currentDate = item.Time.ToShortDateString();
                }
                else
                {
                    lastStatus = "OUT";
                    var inDateTime = DateTime.Parse(_httpContextAccessor.HttpContext.Session.GetString("TempTime"));
                    var outDateTime = item.Time;
                    currentDate = inDateTime.ToShortDateString();
                    TimeSpan tempHour = outDateTime.Subtract(inDateTime);
                     totalHours = totalHours + tempHour;
                }
            }

            if (Status == "IN")
            {
                var res2 = res1.Where(x => x.Status == "IN").Select(x => x.Time).LastOrDefault();
                var lastInTime = res1.Select(x => x.Time).LastOrDefault();
                TimeSpan tt = DateTime.Now.Subtract(lastInTime);
                totalHours = totalHours + tt;
                SubcalculateAttendance(currentDate, totalHours);
            }
            if (Status == "OUT")
            {
                SubcalculateAttendance(currentDate, totalHours);
            }
            return userLog;
        }

        public void SubcalculateAttendance(string currentDate, TimeSpan totalHours)
        {
            var dateExist = ctx.AttendenceLogs.ToList().Where(x => x.Date == DateTime.Parse(currentDate));//.Select(x => x.Date).FirstOrDefault();
            DateTime dateExist1 = dateExist.Where(x => x.UserId == _httpContextAccessor.HttpContext.Session.GetInt32("UserId")).Select(x => x.Date).FirstOrDefault();
            var Id1 = ctx.AttendenceLogs.ToList().Where(x => x.Date == DateTime.Parse(currentDate));
            var Id = Id1.Where(x => x.UserId == _httpContextAccessor.HttpContext.Session.GetInt32("UserId")).Select(x => x.Id).FirstOrDefault();
            AttendenceLog attendenceLog = new AttendenceLog();
            attendenceLog.UserId = Convert.ToInt32(_httpContextAccessor.HttpContext.Session.GetInt32("UserId"));
            attendenceLog.TotalHours = $"{totalHours.Hours}:{totalHours.Minutes}:{totalHours.Seconds}";
            attendenceLog.Date = DateTime.Parse(currentDate);
            if (dateExist1.ToShortDateString() != "01-01-0001")
            {
                attendenceLog.Id = Id;
                var info = ctx.AttendenceLogs.Find(Id);
                ctx.Entry(info).CurrentValues.SetValues(attendenceLog);
                ctx.SaveChanges();
            }
            else
            {
                ctx.AttendenceLogs.Add(attendenceLog);
                ctx.SaveChanges();
            }
        }
        public string DecryptAsync(string text)
        {
            var textToDecrypt = text;
            string toReturn = "";
            string publickey = "12345678";
            string secretkey = "87654321";
            byte[] privatekeyByte = { };
            privatekeyByte = System.Text.Encoding.UTF8.GetBytes(secretkey);
            byte[] publickeybyte = { };
            publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);
            MemoryStream ms = null;
            CryptoStream cs = null;
            byte[] inputbyteArray = new byte[textToDecrypt.Replace(" ", "+").Length];
            inputbyteArray = Convert.FromBase64String(textToDecrypt.Replace(" ", "+"));
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                ms = new MemoryStream();
                cs = new CryptoStream(ms, des.CreateDecryptor(publickeybyte, privatekeyByte), CryptoStreamMode.Write);
                cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                cs.FlushFinalBlock();
                Encoding encoding = Encoding.UTF8;
                toReturn = encoding.GetString(ms.ToArray());
            }
            return toReturn;
        }

    }
}
