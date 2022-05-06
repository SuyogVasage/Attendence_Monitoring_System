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
        //[HttpPost]
        //public IActionResult Get(UserDetail user)
        //{
        //    return RedirectToAction("Edit");
        //}

        public IActionResult Edit(int Id)
        {
            var res = userDetailServ.GetAsync(Id).Result;
            //HttpContext.Session.SetInt32("Id", Id);
            int? SectionId = userDetailServ.GetAsync().Result.Where(x => x.Id == Id).Select(x => x.SectionId).FirstOrDefault();
            int sectionId = Convert.ToInt32(SectionId);
            HttpContext.Session.SetInt32("SectionId", sectionId);
            return View(res);
        }

        [HttpPost]
       public IActionResult Edit(UserDetail userDetail)
        {
            //int Id = Convert.ToInt32(HttpContext.Session.GetInt32("Id"));
            userDetail.UserId = 1002;
            userDetail.SectionId = HttpContext.Session.GetInt32("SectionId");
            // var res = userDetailServ.GetAsync().Result.Where(x => x.UserId == userId).OrderBy(x => x.SectionId);
            var res = userDetailServ.UpdateAsync(userDetail.Id, userDetail);
            
            return RedirectToAction("Get");
        }




    }
}
