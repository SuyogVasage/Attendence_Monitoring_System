namespace Attendence_Monitoring_System.Services
{
    public class DataAccess
    {
        private readonly Attendence_Monitoring_SystemContext ctx;

        public DataAccess(Attendence_Monitoring_SystemContext ctx)
        {
            this.ctx = ctx;
        }


    }
}
