namespace Attendence_Monitoring_System.Services
{
    public interface IDataAccess
    {
        EmployeeList EmpController(string SearchOption, string SearchString);
        int calculatTime(out int hr, out int min, out int sec, out int? RoleId);
        UserLog calculateAttendance(string Status);
        void CalculateRealTime(int? UserId, int flag);
        void SubcalculateAttendance(string currentDate, TimeSpan totalHours);
        string DecryptAsync(string text);
    }
}
