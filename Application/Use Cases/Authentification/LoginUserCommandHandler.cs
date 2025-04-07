using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.Authentification
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<LoginResponse>>
    {
        private readonly IMedicRepository medicRepository;
        private readonly IPatientRepository patientRepository;
        private readonly IAdminRepository adminRepository;
        public LoginUserCommandHandler(IMedicRepository medicRepository, IPatientRepository patientRepository, IAdminRepository adminRepository)
        {
            this.medicRepository = medicRepository;
            this.patientRepository = patientRepository;
            this.adminRepository = adminRepository;
        }

        public async Task<Result<LoginResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            string errorMessage = "No account found with this email address. Please try again or sign up.";
            var medicResult = await medicRepository.Login(request.Email, request.Password);
            if (medicResult.IsSuccess)
            {
                return Result<LoginResponse>.Success(medicResult.Data);
            }
            else if (medicResult.ErrorMessage != "No account found with this email address. Please try again or sign up.")
            {
                errorMessage = medicResult.ErrorMessage;
            }

            var patientResult = await patientRepository.Login(request.Email, request.Password);
            if (patientResult.IsSuccess)
            {
                return Result<LoginResponse>.Success(patientResult.Data);
            }
            else if (patientResult.ErrorMessage != "No account found with this email address. Please try again or sign up.")
            {
                errorMessage = patientResult.ErrorMessage;
            }
          
            var adminResult = await adminRepository.Login(request.Email, request.Password);
            if (adminResult.IsSuccess)
            {
                return Result<LoginResponse>.Success(adminResult.Data);
            }
            else if(adminResult.ErrorMessage != "No account found with this email address. Please try again or sign up.")
            {
                errorMessage = adminResult.ErrorMessage;
            }
    

            return Result<LoginResponse>.Failure(errorMessage);
        }
    }
}
