using Microsoft.AspNetCore.Mvc;

namespace Attendence_Monitoring_System.Controllers
{

    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
