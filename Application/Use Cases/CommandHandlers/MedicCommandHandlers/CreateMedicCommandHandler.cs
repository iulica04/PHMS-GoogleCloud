using Application.Commands.Medic;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services;
using MediatR;


namespace Application.CommandHandlers.MedicCommandHandlers
{
    public class CreateMedicCommandHandler : IRequestHandler<CreateMedicCommand, Result<Guid>>
    {
        private readonly IMedicRepository repository;
        private readonly IMapper mapper;

        public CreateMedicCommandHandler(IMedicRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Result<Guid>> Handle(CreateMedicCommand request, CancellationToken cancellationToken)
        {

            var medic = mapper.Map<Medic>(request);
            medic.PasswordHash = PasswordHasher.HashPassword(request.Password);
            var result = await repository.AddAsync(medic);
            if (result.IsSuccess)
            {
                return Result<Guid>.Success(result.Data);
            }
            return Result<Guid>.Failure(result.ErrorMessage);
        }
    }
}
