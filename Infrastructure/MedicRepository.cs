using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Domain.Services;

namespace Infrastructure
{
    public class MedicRepository : IMedicRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration configuration;

        public MedicRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public async Task<Result<LoginResponse>> Login(string email, string password)
        {
            var existingMedic = await context.Medics.SingleOrDefaultAsync(x => x.Email == email);
            if (existingMedic is null)
            {
                return Result<LoginResponse>.Failure("No account found with this email address. Please try again or sign up.");
            }
        
            if (!PasswordHasher.VerifyPassword(password, existingMedic.PasswordHash))
            {
                return Result<LoginResponse>.Failure("The password you entered is incorrect. Please try again.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, existingMedic.Id.ToString()),
            new Claim(ClaimTypes.Role, "Medic"),
        }),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            LoginResponse response = new LoginResponse
            {
                Token = tokenString,
                Id = existingMedic.Id,
                Role = "Medic"
            };

            return Result<LoginResponse>.Success(response);
        }


        public async Task<Result<Guid>> AddAsync(Medic medic)
        {
            try
            {
                await context.Medics.AddAsync(medic);
                await context.SaveChangesAsync();
                return Result<Guid>.Success(medic.Id);

            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure(ex.InnerException!.ToString());
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var medic = await context.Medics.FirstOrDefaultAsync(x => x.Id == id);
            if (medic != null)
            {
                context.Medics.Remove(medic);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Medic>> GetAllAsync()
        {
            return await context.Medics.ToListAsync();
        }

        public async Task<Medic?> GetByIdAsync(Guid id)
        {
            return await context.Medics.FindAsync(id);
        }

        public Task UpdateAsync(Medic medic)
        {
            context.Medics.Update(medic);
            return context.SaveChangesAsync();
        }
    }
}