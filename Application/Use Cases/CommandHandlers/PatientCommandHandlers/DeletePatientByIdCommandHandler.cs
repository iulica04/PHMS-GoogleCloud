using Application.Commands.Patient;
using Domain.Common;
using Domain.Repositories;
using MediatR;


namespace Application.CommandHandlers.PatientCommandHandlers
{
    public class DeletePatientByIdCommandHandler : IRequestHandler<DeletePatientByIdCommand, Result<Unit>>
    {
        private readonly IPatientRepository repository;

        public DeletePatientByIdCommandHandler(IPatientRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Result<Unit>> Handle(DeletePatientByIdCommand request, CancellationToken cancellationToken)
        {
            var patient = await repository.GetByIdAsync(request.Id);
            if (patient == null)
            {
                return Result<Unit>.Failure("Patient not found");
            }
            await repository.DeleteAsync(request.Id);
            return Result<Unit>.Success(Unit.Value);
        }
    }
}
