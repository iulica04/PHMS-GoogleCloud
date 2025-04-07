using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IConsultationRepository
    {
        Task<Result<Guid>> RequestConsultation(Consultation consultation);
        Task<Consultation> GetByIdAsync(Guid id);
        Task<List<Consultation>> GetAllAsync();
        Task UpdateAsync(Consultation consultation);
        Task DeleteAsync(Guid id);
    }
}
