namespace Attendence_Monitoring_System.Services
{
    public class AttendenceLogService : IService<AttendenceLog, int>
    {
        private readonly Attendence_Monitoring_SystemContext ctx;

        public AttendenceLogService(Attendence_Monitoring_SystemContext ctx)
        {
            this.ctx = ctx;
        }
        async Task<AttendenceLog> IService<AttendenceLog, int>.CreateAsync(AttendenceLog entity)
        {
            var result = await ctx.AttendenceLogs.AddAsync(entity);
            await ctx.SaveChangesAsync();
            return result.Entity;
        }

        async Task<IEnumerable<AttendenceLog>> IService<AttendenceLog, int>.GetAsync()
        {
            return await ctx.AttendenceLogs.ToListAsync();
        }

        Task<AttendenceLog> IService<AttendenceLog, int>.GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        async Task<AttendenceLog> IService<AttendenceLog, int>.UpdateAsync(int id, AttendenceLog entity)
        {
            var info = await ctx.AttendenceLogs.FindAsync(id);
            if (info == null)
            {
                return null;
            }
            ctx.Entry(info).CurrentValues.SetValues(entity);
            await ctx.SaveChangesAsync();
            return info;
        }
    }
}
