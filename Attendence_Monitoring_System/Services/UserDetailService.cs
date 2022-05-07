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

        async Task<UserDetail> IService<UserDetail, int>.GetAsync(int id)
        {
            var result = await ctx.UserDetails.FindAsync(id);
            return result;
        }

        async Task<UserDetail> IService<UserDetail, int>.UpdateAsync(int id, UserDetail entity)
        {
            var info = await ctx.UserDetails.FindAsync(id);
            if (info == null)
            {
                return null;
            }
            ctx.Entry(info).CurrentValues.SetValues(entity);
            //info.Id = entity.Id;
            //info.SectionId = entity.SectionId;
            //info.UserId = entity.UserId;
            //info.KeyName = entity.KeyName;
            //info.Value = entity.Value;
            await ctx.SaveChangesAsync();
            return info;
        }

        public UserDetail Update1Async(int id, UserDetail entity)
        {
            var info = ctx.UserDetails.Find(id);
            if (info == null)
            {
                return null;
            }
            ctx.Entry(info).CurrentValues.SetValues(entity);
            //info.Id = entity.Id;
            //info.SectionId = entity.SectionId;
            //info.UserId = entity.UserId;
            //info.KeyName = entity.KeyName;
            //info.Value = entity.Value;
            ctx.SaveChanges();
            return info;
        }
    }
}
