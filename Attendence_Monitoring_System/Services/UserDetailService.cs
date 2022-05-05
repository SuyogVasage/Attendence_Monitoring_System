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
            var result = await ctx.UserDetails.ToListAsync();
            return result;
        }

        Task<UserDetail> IService<UserDetail, int>.GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<UserDetail> IService<UserDetail, int>.UpdateAsync(int id, UserDetail entity)
        {
            throw new NotImplementedException();
        }
    }
}
