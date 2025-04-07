using Domain.Common;
using Domain.Repositories;
using Domain.Services;
using MediatR;

namespace Application.Use_Cases.ResetPassword
{
    internal class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result<Unit>>
    {
        private readonly IPatientRepository repository;
        private readonly IValidationTokenService tokenService;

        public ResetPasswordCommandHandler(IPatientRepository repository, IValidationTokenService tokenService)
        {
            this.repository = repository;
            this.tokenService = tokenService;
        }
        public async Task<Result<Unit>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {

            if (!await tokenService.ValidateResetTokenAsync(request.Email, request.Token))
            {
                return Result<Unit>.Failure("Invalid token");
            }

            var patient = await repository.GetByEmailAsync(request.Email);
            if (patient == null)
            {
                return Result<Unit>.Failure("Email not found");
            }

            patient.PasswordHash = PasswordHasher.HashPassword(request.NewPassword);
            await repository.UpdateAsync(patient);
            await tokenService.DeleteResetTokenAsync(request.Email);
            return Result<Unit>.Success(Unit.Value);
        }
    }
}
