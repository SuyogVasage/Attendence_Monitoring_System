namespace Attendence_Monitoring_System.Services
{
    public class UserDetailService : IService<UserDetail, int>
    {
        private readonly Attendence_Monitoring_SystemContext ctx;

        public UserDetailService(Attendence_Monitoring_SystemContext ctx)
        {
            this.ctx = ctx;
        }

        Task<UserDetail> IService<UserDetail, int>.CreateAsync(UserDetail entity)
        {
            throw new NotImplementedException();
        }

        async Task<IEnumerable<UserDetail>> IService<UserDetail, int>.GetAsync()
        {
            return await ctx.UserDetails.ToListAsync();
        }

        async Task<UserDetail> IService<UserDetail, int>.GetAsync(int id)
        {
            return await ctx.UserDetails.FindAsync(id);
        }

        async Task<UserDetail> IService<UserDetail, int>.UpdateAsync(int id, UserDetail entity)
        {
            var info = await ctx.UserDetails.FindAsync(id);
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
