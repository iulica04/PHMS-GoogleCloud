using Application.Commands.MedicalConditionCommands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.CommandHandlers.MedicalConditionCommandHandlers
{
    public class CreateMedicalConditionCommandHandler : IRequestHandler<CreateMedicalConditionCommand, Result<Guid>>
    {
        private readonly IMedicalConditionRepository medicalConditionRepository;
        private readonly ITreatmentRepository treatmentRepository;
        private readonly IMedicationRepository medicationRepository;
        private readonly IMapper mapper;

        public CreateMedicalConditionCommandHandler(IMedicalConditionRepository medicalConditionRepository, ITreatmentRepository treatmentRepository, IMedicationRepository medicationRepository, IMapper mapper)
        {
            this.medicalConditionRepository = medicalConditionRepository;
            this.treatmentRepository = treatmentRepository;
            this.medicationRepository = medicationRepository;
            this.mapper = mapper;
        }

        public async Task<Result<Guid>> Handle(CreateMedicalConditionCommand request, CancellationToken cancellationToken)
        {
            var medicalCondition = mapper.Map<MedicalCondition>(request);

            var medicalConditionResult = await medicalConditionRepository.AddAsync(medicalCondition);
            if (!medicalConditionResult.IsSuccess)
            {
                return Result<Guid>.Failure(medicalConditionResult.ErrorMessage);
            }

            foreach (var treatmentDto in request.Treatments)
            {
                var treatment = mapper.Map<Treatment>(treatmentDto);
                treatment.MedicalConditionId = medicalConditionResult.Data;

                var treatmentResult = await treatmentRepository.AddAsync(treatment);
                if (!treatmentResult.IsSuccess)
                {
                    return Result<Guid>.Failure(treatmentResult.ErrorMessage);
                }

                foreach (var medicationDto in treatmentDto.Medications)
                {
                    var medication = mapper.Map<Medication>(medicationDto);
                    medication.TreatmentId = treatmentResult.Data;

                    var medicationResult = await medicationRepository.AddAsync(medication);
                    if (!medicationResult.IsSuccess)
                    {
                        return Result<Guid>.Failure(medicationResult.ErrorMessage);
                    }
                }
            }

            return Result<Guid>.Success(medicalConditionResult.Data);
        }
    }
}