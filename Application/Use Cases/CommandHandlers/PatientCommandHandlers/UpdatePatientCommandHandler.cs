using Application.Commands.Patient;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using Domain.Services;
using MediatR;

namespace Application.CommandHandlers.PatientCommandHandlers
{
    public class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand, Result<Unit>>
    {
        private readonly IPatientRepository patientRepository;
        private readonly IMapper mapper;

        public UpdatePatientCommandHandler(IPatientRepository patientRepository, IMapper mapper)
        {
            this.patientRepository = patientRepository;
            this.mapper = mapper;
        }
        public async Task<Result<Unit>> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = await patientRepository.GetByIdAsync(request.Id);
            if (patient == null)
            {
                return Result<Unit>.Failure("Patient not found");
            }
            if (!string.IsNullOrEmpty(request.Password))
            {
                patient.PasswordHash = PasswordHasher.HashPassword(request.Password);
            }

            var updatedPatient = mapper.Map(request, patient);
            await patientRepository.UpdateAsync(updatedPatient);
            return Result<Unit>.Success(Unit.Value);
        }
    }
}
