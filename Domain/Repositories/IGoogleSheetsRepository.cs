using Domain.Entities;

namespace Domain.Repositories
{
    public interface IGoogleSheetsRepository
    {
        Task<string> ExportPatientsToSheetAsync(List<Patient> patients);
    }
}
