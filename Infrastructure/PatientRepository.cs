using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration configuration;

        public PatientRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }
        public async Task<Result<Guid>> AddAsync(Patient patient)
        {
            try
            {
                await context.Patients.AddAsync(patient);
                await context.SaveChangesAsync();
                return Result<Guid>.Success(patient.Id);
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure(ex.InnerException!.ToString());
            }
        }

        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            return await context.Patients.ToListAsync();
        }

        public async Task<IEnumerable<Patient>> GetAllWithMedicalConditionAsync()
        {
            return await context.Patients
                        .Include(p => p.MedicalConditions)
                        .ToListAsync();
        }

        public async Task<Patient?> GetByIdAsync(Guid id)
        {
            return await context.Patients.FindAsync(id);
        }

        public async Task UpdateAsync(Patient patient)
        {
            context.Entry(patient).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var patient = await context.Patients.FindAsync(id);
            if (patient != null)
            {
                context.Patients.Remove(patient);
                await context.SaveChangesAsync();
            }
        }


        public async Task<Result<LoginResponse>> Login(string email, string password)
        {
            var existingPatient = await context.Patients.SingleOrDefaultAsync(x => x.Email == email);
            if (existingPatient == null)
            {
                return Result<LoginResponse>.Failure("No account found with this email address. Please try again or sign up.");
            }


            if (!PasswordHasher.VerifyPassword(password, existingPatient.PasswordHash))
            {
                return Result<LoginResponse>.Failure("The password you entered is incorrect. Please try again.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, existingPatient.Id.ToString()),
                    new Claim(ClaimTypes.Role,"")
                }),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            LoginResponse response = new LoginResponse
            {
                Token = tokenString,
                Id = existingPatient.Id,
                Role = "Patient"
            };

            return Result<LoginResponse>.Success(response);
        }
        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await context.Patients.AnyAsync(p => p.Email == email);
        }

        public async Task<Patient?> GetByEmailAsync(string email)
        {
            return await context.Patients.FirstOrDefaultAsync(p => p.Email == email);
        }

    }
}
