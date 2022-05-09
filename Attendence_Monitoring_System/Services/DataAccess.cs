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
