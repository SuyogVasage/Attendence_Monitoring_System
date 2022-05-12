using Microsoft.AspNetCore.Mvc;

namespace Attendence_Monitoring_System.Controllers
{
    public class UserDetailController : Controller
    {
        private readonly IService<UserDetail, int> userDetailServ;
        private readonly Attendence_Monitoring_SystemContext ctx;
        public ProfileValidation profileValidation;
        public UserDetailController(IService<UserDetail, int> userDetailServ, Attendence_Monitoring_SystemContext ctx)
        {
            this.userDetailServ = userDetailServ;
            this.ctx = ctx;
            profileValidation = new ProfileValidation();
        }

        public IActionResult Get()
        {
            var res = userDetailServ.GetAsync().Result.Where(x => x.UserId == HttpContext.Session.GetInt32("UserId")).OrderBy(x => x.SectionId);
            var photo = res.Where(x => x.KeyName == "Img Path").Select(x=>x.Value).FirstOrDefault();
            ViewBag.photo = photo;
            return View(res);
        }

        public IActionResult Edit(int Id)
        {
            var res = userDetailServ.GetAsync(Id).Result;
            int? SectionId = userDetailServ.GetAsync().Result.Where(x => x.Id == Id).Select(x => x.SectionId).FirstOrDefault();
            int sectionId = Convert.ToInt32(SectionId);
            HttpContext.Session.SetInt32("SectionId", sectionId);
            return View(res);
        }

        [HttpPost]
       public IActionResult Edit(UserDetail userDetail)
        {
            string ErrorMessage = String.Empty;
            bool validResult = profileValidation.Validation(userDetail, out ErrorMessage);
            if(validResult == false)
            {
                ViewBag.Error = ErrorMessage;
                return View(userDetail);
            }
            userDetail.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            userDetail.SectionId = HttpContext.Session.GetInt32("SectionId");
            var info = ctx.UserDetails.Find(userDetail.Id);
            info.Id = userDetail.Id;
            info.SectionId = userDetail.SectionId;
            info.UserId = userDetail.UserId;
            info.KeyName = userDetail.KeyName;
            info.Value = userDetail.Value;
            ctx.SaveChanges();
            return RedirectToAction("Get");
        }

    }
}
