using Application.Commands.MedicationCommand;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

public class CreateMedicationCommandHandler : IRequestHandler<CreateMedicationCommand, Result<Guid>>
{
    private readonly IMedicationRepository medicationRepository;
    private readonly IMapper mapper;

    public CreateMedicationCommandHandler(IMedicationRepository medicationRepository, IMapper mapper)
    {
        this.medicationRepository = medicationRepository;
        this.mapper = mapper;
    }

    public async Task<Result<Guid>> Handle(CreateMedicationCommand request, CancellationToken cancellationToken)
    {
        var medication = mapper.Map<Medication>(request);

        var result = await medicationRepository.AddAsync(medication);
        if (!result.IsSuccess)
        {
            return Result<Guid>.Failure(result.ErrorMessage);
        }

        return Result<Guid>.Success(result.Data);
    }
}
