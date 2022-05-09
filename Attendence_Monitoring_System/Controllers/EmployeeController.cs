

namespace Attendence_Monitoring_System.Controllers
{

    public class EmployeeController : Controller
    {
        private readonly Attendence_Monitoring_SystemContext ctx;
        public DataAccess dataAccess;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EmployeeController(IHttpContextAccessor _httpContextAccessor, Attendence_Monitoring_SystemContext ctx)
        {
            this.ctx = ctx;
            this._httpContextAccessor = _httpContextAccessor;
            dataAccess = new DataAccess(_httpContextAccessor,ctx);
        }
        public IActionResult ViewEmployee(string SearchOption, string SearchString)
        {
            var empList = dataAccess.EmpController(SearchOption, SearchString);
            ListItem();
            return View(empList);
        }

        public IActionResult GetEmployee(string SearchOption, string SearchString)
        {
            ListItem();
            return RedirectToAction("ViewEmployee", new { SearchOption = SearchOption, SearchString = SearchString });
        }


        public void ListItem()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "ByName", Value = "Name" });
            items.Add(new SelectListItem { Text = "ByEmpId", Value = "EmpId" });
            ViewBag.Options = items;
        }
    }


   
}
