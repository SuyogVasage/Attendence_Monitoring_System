﻿using Microsoft.AspNetCore.Mvc.RazorPages;

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
        //From EmployeeController
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

        //From UserLog=>Create
        public int calculatTime(out int hr, out int min, out int sec, out int? RoleId)
        {
            
            int? UserId = _httpContextAccessor.HttpContext.Session.GetInt32("UserId");
            RoleId =_httpContextAccessor.HttpContext.Session.GetInt32("RoleId");

            //For New Day
            var lastDate = ctx.UserLogs.ToList().Where(x => x.UserId == UserId).Select(x => x.Time).LastOrDefault().ToShortDateString();
            if(lastDate != DateTime.Now.ToShortDateString())
            {
                hr = 0;
                min= 0;
                sec = 0; 
                return 2;
            }

            var userLogs = ctx.UserLogs.ToList().Where(x => x.UserId == _httpContextAccessor.HttpContext.Session.GetInt32("UserId"));
            var userLogsByDate = userLogs.Where(x => x.Time.ToShortDateString() == DateTime.Now.ToShortDateString());
            TimeSpan middleTime;
            TimeSpan TotalTime = TimeSpan.Zero;
            string lastStatus = String.Empty;
            //Calculating Total TimeSpan
            foreach (var item in userLogsByDate)
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
                var res2 = userLogsByDate.Where(x => x.Status == "IN").Select(x => x.Time).LastOrDefault();
                var lastInTime = userLogsByDate.Select(x => x.Time).LastOrDefault();
                TimeSpan tt = DateTime.Now.Subtract(lastInTime);
                string ttt = $"{tt.Hours}:{tt.Minutes}:{tt.Seconds}";
                TotalTime = TotalTime + tt;
            }
            //Getting only hours, minutes, seconds from TimeSpan
            hr = Convert.ToInt32(TotalTime.Hours);
            min = Convert.ToInt32(TotalTime.Minutes);
            sec = Convert.ToInt32(TotalTime.Seconds);
            //Returning the Status by int Notation
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

        //From UserLog=>Create
        public UserLog calculateAttendance(string Status)
        {
            UserLog userLog = new UserLog();
            userLog.UserId = _httpContextAccessor.HttpContext.Session.GetInt32("UserId");
            userLog.Time = DateTime.Now;
            userLog.Status = Status;
            //Adding Data in UserLog Table
            var temp = ctx.UserLogs.Add(userLog);
            ctx.SaveChanges();

            string lastStatus = String.Empty;
            //userLogs by UserId
            var userLogs = ctx.UserLogs.ToList().Where(x => x.UserId == _httpContextAccessor.HttpContext.Session.GetInt32("UserId"));
            //userLogs by Date
            var userLogsByDate = userLogs.Where(x => x.Time.ToShortDateString() == DateTime.Now.ToShortDateString());
            
            
            TimeSpan totalHours = TimeSpan.Zero;
            string currentDate = String.Empty;
            foreach (var item in userLogsByDate)
            {
                if (item.Status == "IN")
                {
                    //Saving In Time
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
                    //Calculating Timespan Between outTime and inTime
                    TimeSpan tempHour = outDateTime.Subtract(inDateTime);
                    //Saving Total Time
                     totalHours = totalHours + tempHour;
                }
            }

            //If last Status is IN calculate Time Between DateTime.Now and Last In Time
            //And add that time to totalHours
            if (Status == "IN")
            {
                var lastInTime = userLogsByDate.Select(x => x.Time).LastOrDefault();
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

        //Common Method for Both IN and OUT 
        //To Update or Create Attendence Log in DB
        public void SubcalculateAttendance(string currentDate, TimeSpan totalHours)
        {
            //IEnumerable of Dates in Attendence Log Table
            var dateExist = ctx.AttendenceLogs.ToList().Where(x => x.Date == DateTime.Parse(currentDate));
            //Date for particular User
            DateTime dateExist1 = dateExist.Where(x => x.UserId == _httpContextAccessor.HttpContext.Session.GetInt32("UserId")).Select(x => x.Date).FirstOrDefault();
            var Ids = ctx.AttendenceLogs.ToList().Where(x => x.Date == DateTime.Parse(currentDate));
            //Finding ID (PK of Table) To Update the data
            var Id = Ids.Where(x => x.UserId == _httpContextAccessor.HttpContext.Session.GetInt32("UserId")).Select(x => x.Id).FirstOrDefault();
            AttendenceLog attendenceLog = new AttendenceLog();
            attendenceLog.UserId = Convert.ToInt32(_httpContextAccessor.HttpContext.Session.GetInt32("UserId"));
            attendenceLog.TotalHours = $"{totalHours.Hours}:{totalHours.Minutes}:{totalHours.Seconds}";
            attendenceLog.Date = DateTime.Parse(currentDate);
            //If Date Exist then Update Otherwise Add
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

        //Decrypting the Password
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
