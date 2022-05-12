using Microsoft.AspNetCore.Mvc;

namespace Attendence_Monitoring_System.Controllers
{
    public class RegularizationController : Controller
    {
        private readonly IService<Regularization, int> regularizationServ;
        public RegularizationController(IService<Regularization, int> regularizationServ)
        {
            this.regularizationServ = regularizationServ;
        }
        public IActionResult Create()
        {
            Regularization regularization = new Regularization();
            ListItem();
            return View(regularization);
        }

        //Creating regularization request 
        [HttpPost]
        public IActionResult Create(Regularization regularization)
        {
            regularization.UserId = HttpContext.Session.GetInt32("UserId");
            regularization.Status = "Pending";
            TimeSpan TotalHours = regularization.OutTime.Subtract(regularization.InTime);
            regularization.TotalHours = $"{TotalHours.Hours}:{TotalHours.Minutes}:{TotalHours.Seconds}";
            var result = regularizationServ.CreateAsync(regularization).Result;
            return RedirectToAction("Get", "AttendenceLog");
        }

        //Admin will see only pending requests
        public IActionResult Get()
        {
            var result = regularizationServ.GetAsync().Result.Where(x=>x.Status=="Pending");  
            return View(result);
        }

        // Approving regularization by Admin
        public IActionResult Edit(int Id)
        {
            var item = regularizationServ.GetAsync().Result.Where(x=>x.Id ==Id).FirstOrDefault();
            item.Status = "Approved";
            var result = regularizationServ.UpdateAsync(Id, item);
            HttpContext.Session.SetObject<Regularization>("UpdateData", item);
            return RedirectToAction("Edit", "AttendenceLog");
        }

        //User can see all requests of him
        public IActionResult GetforUser()
        {
            var result = regularizationServ.GetAsync().Result.Where(x => x.UserId == HttpContext.Session.GetInt32("UserId"));
            return View(result);
        }


        //DropDown for Reason on regularization
        public void ListItem()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Forgot To Check-In", Value = "Forgot To Check-In" });
            items.Add(new SelectListItem { Text = "Forgot To Check-Out", Value = "Forgot To Check-Out" });
            ViewBag.Reasons = items;
        }
    }
}
