namespace Attendence_Monitoring_System.Services
{
    public class UserService : IService<User, int>
    {
        private readonly Attendence_Monitoring_SystemContext ctx;
        public UserService(Attendence_Monitoring_SystemContext ctx)
        {
            this.ctx = ctx;    
        }

        Task<User> IService<User, int>.CreateAsync(User entity)
        {
            throw new NotImplementedException();
        }

        async Task<IEnumerable<User>> IService<User, int>.GetAsync()
        {
            return await ctx.Users.ToListAsync();
        }

        Task<User> IService<User, int>.GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<User> IService<User, int>.UpdateAsync(int id, User entity)
        {
            throw new NotImplementedException();
        }
    }
}
