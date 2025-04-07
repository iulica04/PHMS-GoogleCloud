using Application.Queries;
using Domain.Repositories;
using MediatR;

namespace Application.QueryHandlers
{
    public class CheckEmailQueryHandler : IRequestHandler<CheckEmailQuery, bool>
    {
        private readonly IPatientRepository repository;
        public CheckEmailQueryHandler(IPatientRepository patientRepository)
        {
            this.repository = patientRepository;
        }
        public async Task<bool> Handle(CheckEmailQuery request, CancellationToken cancellationToken)
        {
            return await repository.ExistsByEmailAsync(request.Email);
        }
    }
}
