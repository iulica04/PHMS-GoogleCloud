using System.Linq.Expressions;
using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IMedicalConditionRepository
    {
        Task<IEnumerable<MedicalCondition>> GetAllAsync(Expression<Func<MedicalCondition, bool>>? filter = null);
        Task<MedicalCondition?> GetByIdAsync(Expression<Func<MedicalCondition, bool>> filter);
        Task<Result<Guid>> AddAsync(MedicalCondition medicalCondition);
        Task UpdateAsync(MedicalCondition medicalCondition);
        Task DeleteAsync(Guid id);

    }
}
