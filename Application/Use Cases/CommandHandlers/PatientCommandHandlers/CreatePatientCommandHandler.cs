using Application.Commands.Patient;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services;
using MediatR;

namespace Application.CommandHandlers.PatientCommandHandlers
{
    public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, Result<Guid>>
    {
        private readonly IPatientRepository repository;
        private readonly IMapper mapper;

        public CreatePatientCommandHandler(IPatientRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Result<Guid>> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {
            var patien = mapper.Map<Patient>(request);
            patien.PasswordHash = PasswordHasher.HashPassword(request.Password);
            var result = await repository.AddAsync(patien);
            if (result.IsSuccess)
            {
                return Result<Guid>.Success(result.Data);
            }
            return Result<Guid>.Failure(result.ErrorMessage);
        }
    }
}