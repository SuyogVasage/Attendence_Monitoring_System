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
            var lastStatus = ctx.UserLogs.ToList().Where(x => x.UserId == UserId).Select(x => x.Status).LastOrDefault();
            var lastDate = ctx.UserLogs.ToList().Where(x => x.UserId == UserId).Select(x => x.Time).LastOrDefault().ToShortDateString();
            var res = ctx.UserLogs.ToList().Where(x => x.UserId == UserId);
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

        public UserLog calculateAttendance(string Status)
        {
            UserLog userLog = new UserLog();
            userLog.UserId = _httpContextAccessor.HttpContext.Session.GetInt32("UserId");
            userLog.Time = DateTime.Now;
            userLog.Status = Status;
            ctx.UserLogs.Add(userLog);
            ctx.SaveChanges();
            if (Status == "OUT")
            {
                var res = ctx.UserLogs.ToList().Where(x => x.UserId == _httpContextAccessor.HttpContext.Session.GetInt32("UserId"));
                var res1 = res.Where(x => x.Time.ToShortDateString() == DateTime.Now.ToShortDateString());
                double totalHours = 0;
                string currentDate = String.Empty;
                foreach (var item in res1)
                {
                    if (item.Status == "IN")
                    {
                        _httpContextAccessor.HttpContext.Session.SetString("TempTime", item.Time.ToString());
                    }
                    else
                    {
                        var inDateTime = DateTime.Parse(_httpContextAccessor.HttpContext.Session.GetString("TempTime"));
                        var outDateTime = item.Time;
                        currentDate = inDateTime.ToShortDateString();
                        totalHours = (outDateTime - inDateTime).TotalHours;
                        totalHours += totalHours;
                    }
                }
                var dateExist = ctx.AttendenceLogs.ToList().Where(x => x.Date == DateTime.Parse(currentDate));//.Select(x => x.Date).FirstOrDefault();
                var dateExist1 = dateExist.Where(x => x.UserId == _httpContextAccessor.HttpContext.Session.GetInt32("UserId")).Select(x => x.Date).FirstOrDefault();
                var Id = ctx.AttendenceLogs.ToList().Where(x => x.Date == DateTime.Parse(currentDate)).Select(x => x.Id).FirstOrDefault();
                AttendenceLog attendenceLog = new AttendenceLog();
                attendenceLog.UserId = Convert.ToInt32(_httpContextAccessor.HttpContext.Session.GetInt32("UserId"));
                attendenceLog.TotalHours = totalHours;
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
            return userLog;
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
