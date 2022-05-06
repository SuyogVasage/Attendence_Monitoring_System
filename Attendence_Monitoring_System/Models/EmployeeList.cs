namespace Attendence_Monitoring_System.Models
{
    public class EmployeeList
    {
        public string searchOption { get; set; }
        public string searchString { get; set; }

        public IEnumerable<UserDetail> users { get; set; }
    }
}
