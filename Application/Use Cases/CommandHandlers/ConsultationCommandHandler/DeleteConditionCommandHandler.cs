
using Application.Use_Cases.Commands.ConsultationCommands;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.ConsultationCommandHandler
{
    public class DeleteConditionCommandHandler : IRequestHandler<DeleteConsultationCommand, Result<Unit>>
    {
        private readonly IConsultationRepository repository;
        public DeleteConditionCommandHandler(IConsultationRepository repository)
        {
            this.repository = repository;
            
        }
        public async Task<Result<Unit>> Handle(DeleteConsultationCommand request, CancellationToken cancellationToken)
        {
            var consultation = await repository.GetByIdAsync(request.Id);
            if (consultation == null)
            {
                return Result<Unit>.Failure("Consultation not found");
            }
            await repository.DeleteAsync(request.Id);
            return Result<Unit>.Success(Unit.Value);
        }
    }
}
