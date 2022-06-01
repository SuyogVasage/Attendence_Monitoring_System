
namespace Attendence_Monitoring_System.Controllers
{
    public class UserController : Controller
    {
        private readonly IService<User, int> userServ;
        private readonly IDataAccess iDataAccess;

        public UserController(IService<User, int> userServ, IDataAccess iDataAccess)
        {
            this.userServ = userServ;
            this.iDataAccess = iDataAccess;
        }

        public IActionResult Login()
        {
            User user = new User();
            return View(user);
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            user.Email = user.Email.ToLower();
            var userData = userServ.GetAsync().Result.Where(x => x.Email == user.Email).FirstOrDefault();
            if (userData == null)
            {
                ViewBag.Message = "Wrong EmailId";
                return View(user);
            }
            //Decrypting Password from DB
            string decryptedPassword = iDataAccess.DecryptAsync(userData.Password);
            if (user.Password == decryptedPassword)
            {
                //Setting RoleId for Reference
                HttpContext.Session.SetInt32("RoleId", userData.RoleId);
                //Setting UserId for Reference
                HttpContext.Session.SetInt32("UserId", userData.UserId);
                //To Home Page (Timer Page)
                return RedirectToAction("Create", "UserLog");
            }
            else
            {
                ViewBag.Message = "Wrong Password";
                return View(user);
            }
        }

        public IActionResult Logout()
        {
            //Simply Redirecting to Login Page
            return RedirectToAction("Login");
        }

    }
}
