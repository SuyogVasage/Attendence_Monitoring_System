
namespace Attendence_Monitoring_System.Controllers
{

    public class EmployeeController : Controller
    {
        private readonly IDataAccess iDataAccess;
        public EmployeeController(IDataAccess iDataAccess)
        {
            this.iDataAccess = iDataAccess;
        }

        //These two methods will display required employee details for Admin
        public IActionResult ViewEmployee(string SearchOption, string SearchString)
        {
            var empList = iDataAccess.EmpController(SearchOption, SearchString);
            ListItem();
            if (empList.users != null)
            {
                ViewBag.ImgPath = empList.users.Where(x => x.KeyName == "Img Path").Select(x => x.Value).FirstOrDefault();
            }
            return View(empList);
        }

        public IActionResult GetEmployee(string SearchOption, string SearchString)
        {
            ListItem();
            return RedirectToAction("ViewEmployee", new { SearchOption = SearchOption, SearchString = SearchString });
        }

        //DropDown for Choice
        public void ListItem()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "ByName", Value = "Name" });
            items.Add(new SelectListItem { Text = "ByEmpId", Value = "EmpId" });
            ViewBag.Options = items;
        }
    }
}
