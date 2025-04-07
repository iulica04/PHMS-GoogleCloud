using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class MedicationRepository : IMedicationRepository
    {
        public readonly ApplicationDbContext context;
        public MedicationRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Result<Guid>> AddAsync(Medication medication)
        {
            try
            {
                await context.Medications.AddAsync(medication);
                await context.SaveChangesAsync();
                return Result<Guid>.Success(medication.Id);
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure(ex.InnerException!.ToString());
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var medication = await context.Medications.FindAsync(id);
            if (medication != null)
            {
                context.Medications.Remove(medication);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Medication>> GetAllAsync()
        {
            return await context.Medications.ToListAsync();
        }

        public async Task<Medication?> GetByIdAsync(Guid id)
        {
            return await context.Medications.FindAsync(id);
        }

        public async Task UpdateAsync(Medication medication)
        {
            context.Entry(medication).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
