

using Application.Use_Cases.Commands.ConsultationCommands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.ConsultationCommandHandler
{
    public class CreateConsultationCommandHandler : IRequestHandler<CreateConsultationCommand, Result<Guid>>
    {
        private readonly IConsultationRepository repository;
        private readonly IMapper mapper;

        public CreateConsultationCommandHandler(IConsultationRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<Result<Guid>> Handle(CreateConsultationCommand request, CancellationToken cancellationToken)
        {

            var consultation = mapper.Map<Consultation>(request);

            if (consultation == null || consultation.PatientId == Guid.Empty || consultation.MedicId == Guid.Empty)
            {
                return Result<Guid>.Failure("Invalid consultation data.");
            }


            var result = await repository.RequestConsultation(consultation);
            if (result.IsSuccess)
            {
                return Result<Guid>.Success(result.Data);
            }
            return Result<Guid>.Failure(result.ErrorMessage);
        }
    }
}
