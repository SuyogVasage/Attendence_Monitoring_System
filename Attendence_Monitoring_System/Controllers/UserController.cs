

namespace Attendence_Monitoring_System.Controllers
{
    public class UserController : Controller
    {
        private readonly IService<User, int> userServ;

        public UserController(IService<User, int> userServ)
        {
            this.userServ = userServ;
        }

        public IActionResult Login()
        {
            User user = new User();
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            user.Email = user.Email.ToLower();
            var res = userServ.GetAsync().Result.Where(x => x.Email == user.Email).FirstOrDefault();
            if (res == null)
            {
                ViewBag.Message = "Wrong EmailId";
                return View(user);
            }
            if (user.Email == res.Email)
            {
                string decryptedPassword = await DecryptAsync(res.Password);
                if (user.Password == decryptedPassword)
                {
                    if (res.RoleId == 101)
                    {
                        HttpContext.Session.SetInt32("UserId",res.UserId);
                        return RedirectToAction("Create", "UserLog");
                    }
                    else
                    {
                        return RedirectToAction("Privacy", "Home");
                    }
                }
                else
                {
                    ViewBag.Message = "Wrong Password";
                    return View(user);
                }
            }
            else
            {
                ViewBag.Message = "Wrong EmailID";
                return View(user);
            }
        }

        public async Task<string> DecryptAsync(string text)
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
