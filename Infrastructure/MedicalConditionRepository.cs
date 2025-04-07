using System.Linq.Expressions;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class MedicalConditionRepository : IMedicalConditionRepository
    {
        private readonly ApplicationDbContext context;

        public MedicalConditionRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Result<Guid>> AddAsync(MedicalCondition medicalCondition)
        {
            try
            {
                await context.MedicalConditions.AddAsync(medicalCondition);
                await context.SaveChangesAsync();
                return Result<Guid>.Success(medicalCondition.MedicalConditionId);
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure(ex.InnerException!.ToString());
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var medicalCondition = await context.MedicalConditions.FirstOrDefaultAsync(x => x.MedicalConditionId == id);
            if (medicalCondition != null)
            {
                context.MedicalConditions.Remove(medicalCondition);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<MedicalCondition>> GetAllAsync(Expression<Func<MedicalCondition, bool>>? filter = null)
        {
            IQueryable<MedicalCondition> query = context.MedicalConditions
                .Include(mc => mc.Treatments)
                    .ThenInclude(t => t.Medications);

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }

        public async Task<MedicalCondition?> GetByIdAsync(Expression<Func<MedicalCondition, bool>> filter)
        {
            return await context.MedicalConditions.FirstOrDefaultAsync(filter);
        }

        public async Task UpdateAsync(MedicalCondition medicalCondition)
        {
            context.MedicalConditions.Update(medicalCondition);
            await context.SaveChangesAsync();
        }
    }
}
