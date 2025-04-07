using Application.Use_Cases.Commands.PatientCommands;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.PatientCommandHandlers
{
    public class UpdatePatientPasswordCommandHandler : IRequestHandler<UpdatePatientPasswordCommand, Result<Unit>>
    {
        private readonly IPatientRepository patientRepository;
        private readonly IMapper mapper;

        public UpdatePatientPasswordCommandHandler(IPatientRepository patientRepository, IMapper mapper)
        {
            this.patientRepository = patientRepository;
            this.mapper = mapper;
        }
        public async Task<Result<Unit>> Handle(UpdatePatientPasswordCommand request, CancellationToken cancellationToken)
        {
            var patient = await patientRepository.GetByIdAsync(request.PatientId);
            if (patient == null)
            {
                return Result<Unit>.Failure("Patient not found.");
            }

            patient.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            await patientRepository.UpdateAsync(patient);
            return Result<Unit>.Success(Unit.Value);
        }
    }
}
