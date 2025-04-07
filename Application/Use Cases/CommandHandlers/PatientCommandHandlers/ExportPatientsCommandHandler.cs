using Application.Use_Cases.Commands.PatientCommands;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.PatientCommandHandlers
{
    public class ExportPatientsHandler : IRequestHandler<ExportPatientsCommand, string>
    {
        private readonly IGoogleSheetsRepository _sheetsService;
        private readonly IPatientRepository _patientRepository;

        public ExportPatientsHandler(IGoogleSheetsRepository sheetsService, IPatientRepository patientRepository)
        {
            _sheetsService = sheetsService;
            _patientRepository = patientRepository;
        }

        public async Task<string> Handle(ExportPatientsCommand request, CancellationToken cancellationToken)
        {
            var patients = await _patientRepository.GetAllWithMedicalConditionAsync();
            var patientList = patients.ToList(); // Convert IEnumerable<Patient> to List<Patient>
            return await _sheetsService.ExportPatientsToSheetAsync(patientList);
        }
    }
}