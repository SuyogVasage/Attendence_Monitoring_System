namespace Attendence_Monitoring_System.Services
{
    public class UserLogService : IService<UserLog, int>
    {
        private readonly Attendence_Monitoring_SystemContext ctx;
        public UserLogService(Attendence_Monitoring_SystemContext ctx)
        {
            this.ctx = ctx;
        }
        async Task<UserLog> IService<UserLog, int>.CreateAsync(UserLog entity)
        {
            var result = await ctx.UserLogs.AddAsync(entity);
            await ctx.SaveChangesAsync();
            return result.Entity;
        }

        async Task<IEnumerable<UserLog>> IService<UserLog, int>.GetAsync()
        {
            return await ctx.UserLogs.ToListAsync(); ;
        }

        async Task<UserLog> IService<UserLog, int>.GetAsync(int id)
        {
            return await ctx.UserLogs.FindAsync(id); ;
        }

        Task<UserLog> IService<UserLog, int>.UpdateAsync(int id, UserLog entity)
        {
            throw new NotImplementedException();
        }
    }
}
