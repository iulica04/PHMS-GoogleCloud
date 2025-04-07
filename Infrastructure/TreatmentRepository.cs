using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class TreatmentRepository : ITreatmentRepository
    {
        public readonly ApplicationDbContext context;
        public TreatmentRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Result<Guid>> AddAsync(Treatment treatment)
        {
            try
            {
                await context.Treatments.AddAsync(treatment);
                await context.SaveChangesAsync();
                return Result<Guid>.Success(treatment.TreatmentId);
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure(ex.InnerException!.ToString());
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var treatment = await context.Treatments.FindAsync(id);
            if (treatment != null)
            {
                context.Treatments.Remove(treatment);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Treatment>> GetAllAsync()
        {
            return await context.Treatments
                .Include(t => t.Medications) 
                .ToListAsync();
        }

        public async Task<Treatment?> GetByIdAsync(Guid id)
        {
            return await context.Treatments.FindAsync(id);
        }

        public async Task UpdateAsync(Treatment treatment)
        {
            context.Entry(treatment).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task<Treatment?> GetByNameAsync(string name)
        {
            return await context.Set<Treatment>().FirstOrDefaultAsync(t => t.Name == name);
        }
    }
}
