using Microsoft.AspNetCore.Mvc;

namespace Attendence_Monitoring_System.Controllers
{
    public class UserController : Controller
    {
        private readonly IService<User, int> userServ;

        public UserController(IService<User, int> userServ)
        {
            this.userServ = userServ;
        }

        public IActionResult Login(User user)
        {
            var res = userServ.GetAsync().Result.Where(x => x.Email == user.Email).FirstOrDefault();
            if (res == null)
            {
                ViewBag.Message = "Wronge Credential";
                return View(user);
            }
            if (user.Email == res.Email)
            {
                if (user.Password == res.Password)
                {
                    if (res.RoleId == 1)
                    {
                        return RedirectToAction("Home", "Index");
                    }
                    else
                    {
                        return RedirectToAction("Home", "Privacy");
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
    }
}
