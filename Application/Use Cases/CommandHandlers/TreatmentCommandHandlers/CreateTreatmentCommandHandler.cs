using Application.Commands.TreatmentCommands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.CommandHandlers.TreatmentCommandHandler
{
    public class CreateTreatmentCommandHandler : IRequestHandler<CreateTreatmentCommand, Result<Guid>>
    {
        private readonly ITreatmentRepository treatmentRepository;
        private readonly IMedicationRepository medicationRepository;
        private readonly IMapper mapper;

        public CreateTreatmentCommandHandler(ITreatmentRepository treatmentRepository, IMedicationRepository medicationRepository, IMapper mapper)
        {
            this.treatmentRepository = treatmentRepository;
            this.medicationRepository = medicationRepository;
            this.mapper = mapper;
        }

        public async Task<Result<Guid>> Handle(CreateTreatmentCommand request, CancellationToken cancellationToken)
        {
            var treatment = mapper.Map<Treatment>(request);

            var treatmentResult = await treatmentRepository.AddAsync(treatment);
            if (!treatmentResult.IsSuccess)
            {
                return Result<Guid>.Failure(treatmentResult.ErrorMessage);
            }

            foreach (var medicationDto in request.Medications)
            {
                var medication = mapper.Map<Medication>(medicationDto);
                medication.TreatmentId = treatmentResult.Data;

                var medicationResult = await medicationRepository.AddAsync(medication);
                if (!medicationResult.IsSuccess)
                {
                    return Result<Guid>.Failure(medicationResult.ErrorMessage);
                }
            }

            return Result<Guid>.Success(treatmentResult.Data);
        }
    }
}