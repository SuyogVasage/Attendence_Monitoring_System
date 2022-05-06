using Microsoft.AspNetCore.Mvc.Rendering;

namespace Attendence_Monitoring_System.Controllers
{

    public class EmployeeController : Controller
    {
        private readonly IService<UserDetail, int> userDetailServ;
        public EmployeeController(IService<UserDetail, int> userDetailServ)
        {
            this.userDetailServ = userDetailServ;
        }
        public async Task<IActionResult> ViewEmployee(string SearchOption, string SearchString)
        {
            IEnumerable<UserDetail> res1 = new List<UserDetail>();
            switch (SearchOption)
            {
                case "Name":
                    int UserId = userDetailServ.GetAsync().Result.Where(e => e.Value.Contains(SearchString)).Select(x=>x.UserId).FirstOrDefault();
                    res1 = userDetailServ.GetAsync().Result.Where(e => e.UserId == UserId);
                    break;
                case "EmpId":
                    res1 = userDetailServ.GetAsync().Result.Where(e => e.UserId == Convert.ToInt32(SearchString));
                    break;
            }
            int UserId1 = res1.Select(x => x.UserId).FirstOrDefault();
            HttpContext.Session.SetInt32("UserId1", UserId1);
            EmployeeList empList = new EmployeeList();
            empList.users = res1;
            empList.searchOption=SearchOption;
            empList.searchString=SearchString;
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "ByName", Value = "Name" });
            items.Add(new SelectListItem { Text = "ByEmpId", Value = "EmpId" });
            ViewBag.Options = items;
            return View(empList);
        }


        public IActionResult GetEmployee(string SearchOption, string SearchString)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "ByName", Value = "Name" });
            items.Add(new SelectListItem { Text = "ByEmpId", Value = "EmpId" });
            ViewBag.Options = items;
            return RedirectToAction("ViewEmployee", new { SearchOption = SearchOption, SearchString = SearchString });
        }
    }


   
}
