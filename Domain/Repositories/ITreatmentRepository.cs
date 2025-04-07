using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface ITreatmentRepository
    {
        Task<IEnumerable<Treatment>> GetAllAsync();
        Task<Treatment?> GetByIdAsync(Guid id);
        Task<Result<Guid>> AddAsync(Treatment treatment);
        Task UpdateAsync(Treatment treatment);
        Task DeleteAsync(Guid id);
        Task<Treatment?> GetByNameAsync(string name);
    }
}
