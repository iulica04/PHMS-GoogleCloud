using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure
{
    public class ConsultationRepository : IConsultationRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration configuration;

        public ConsultationRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public async Task DeleteAsync(Guid id)
        {
            var consultation = await context.Consultations.FindAsync(id);
            if (consultation != null)
            {
                context.Consultations.Remove(consultation);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<Consultation>> GetAllAsync()
        {
            return await context.Consultations.ToListAsync();
        }

        public async Task<Consultation?> GetByIdAsync(Guid id)
        {
            return await context.Consultations.FindAsync(id);
        }

        public async Task<Result<Guid>> RequestConsultation(Consultation consultation)
        {
            try
            {
                // Verificăm dacă pacientul există
                var patientExists = await context.Patients.AnyAsync(p => p.Id == consultation.PatientId);
                if (!patientExists)
                {
                    return Result<Guid>.Failure("Patient not found.");
                }

                // Verificăm dacă medicul există
                var medicExists = await context.Medics.AnyAsync(m => m.Id == consultation.MedicId);
                if (!medicExists)
                {
                    return Result<Guid>.Failure("Medic not found.");
                }

                // Verificăm dacă medicul este disponibil la data solicitată
                var isMedicAvailable = await context.Consultations
                    .Where(c => c.MedicId == consultation.MedicId && c.Date == consultation.Date)
                    .AnyAsync();
                if (isMedicAvailable)
                {
                    return Result<Guid>.Failure("The medic is not available at the selected date and time.");
                }

          
                await context.Consultations.AddAsync(consultation);
                await context.SaveChangesAsync();

                return Result<Guid>.Success(consultation.Id);
            }
            catch (Exception ex)
            {
                // Capturăm orice alte erori și le returnăm
                return Result<Guid>.Failure($"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task UpdateAsync(Consultation consultation)
        {
            context.Entry(consultation).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
