namespace Attendence_Monitoring_System.Services
{
    public class RegularizationService : IService<Regularization, int>
    {
        private readonly Attendence_Monitoring_SystemContext ctx;
        public RegularizationService(Attendence_Monitoring_SystemContext ctx)
        {
            this.ctx = ctx;
        }
        async Task<Regularization> IService<Regularization, int>.CreateAsync(Regularization entity)
        {
            var result = await ctx.Regularizations.AddAsync(entity);
            await ctx.SaveChangesAsync();
            return result.Entity;
        }

        async Task<IEnumerable<Regularization>> IService<Regularization, int>.GetAsync()
        {
            var result = await ctx.Regularizations.ToListAsync();
            return result;
        }

        Task<Regularization> IService<Regularization, int>.GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        async Task<Regularization> IService<Regularization, int>.UpdateAsync(int id, Regularization entity)
        {
            var info = await ctx.Regularizations.FindAsync(id);
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
