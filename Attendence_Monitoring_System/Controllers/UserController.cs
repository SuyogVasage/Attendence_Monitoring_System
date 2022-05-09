using Microsoft.AspNetCore.Authentication;

namespace Attendence_Monitoring_System.Controllers
{
    public class UserController : Controller
    {
        private readonly IService<User, int> userServ;
        private readonly Attendence_Monitoring_SystemContext ctx;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DataAccess dataAccess;

        public UserController(IService<User, int> userServ, Attendence_Monitoring_SystemContext ctx, IHttpContextAccessor _httpContextAccessor)
        {
            this.userServ = userServ;
            this.ctx = ctx;
            this._httpContextAccessor = _httpContextAccessor;
            dataAccess = new DataAccess(_httpContextAccessor, ctx);
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
            string decryptedPassword = dataAccess.DecryptAsync(res.Password);
            if (user.Password == decryptedPassword)
            {
                HttpContext.Session.SetInt32("RoleId", res.RoleId);
                HttpContext.Session.SetInt32("UserId",res.UserId);
                return RedirectToAction("Create", "UserLog");
            }
            else
            {
                ViewBag.Message = "Wrong Password";
                return View(user);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

    }
}
