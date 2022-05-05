using Microsoft.AspNetCore.Mvc;

namespace Attendence_Monitoring_System.Controllers
{
    public class UserDetailController : Controller
    {
        private readonly IService<UserDetail, int> userDetailServ;

        public UserDetailController(IService<UserDetail, int> userDetailServ)
        {
            this.userDetailServ = userDetailServ;
        }

        public IActionResult Get()
        {
            int userId = 1002;
            var res = userDetailServ.GetAsync().Result.Where(x => x.UserId == userId).OrderBy(x => x.SectionId);
            var photo = res.Where(x => x.KeyName == "Img Path").Select(x=>x.Value).FirstOrDefault();
            ViewBag.photo = photo;
            return View(res);
        }
        [HttpPost]
        public IActionResult Get(UserDetail user)
        {
            return RedirectToAction("Edit");
        }
        public IActionResult Edit()
        {
            UserDetail userDetail = new UserDetail();
            int userId = 1002;
            var res = userDetailServ.GetAsync().Result.Where(x => x.UserId == userId).OrderBy(x => x.SectionId);
            return View(userDetail);
        }

        [HttpPost]
        public IActionResult Edit(UserDetail user)
        {
            return RedirectToAction("Get");
        }




    }
}
