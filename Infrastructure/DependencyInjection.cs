using Domain.Repositories;
using Domain.Services;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, bool useInMemoryDatabaseEnvVar)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IMedicRepository, MedicRepository>();
            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IMedicalConditionRepository, MedicalConditionRepository>();
            services.AddScoped<ITreatmentRepository, TreatmentRepository>();
            services.AddScoped<IMedicationRepository, MedicationRepository>();
            services.AddScoped<IValidationTokenService, ValidationTokenService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddTransient<IConsultationRepository, ConsultationRepository>();
            services.AddScoped<IMedicationExternalService, MedicationExternalService>();
            services.AddScoped<IGoogleCalendarRepository, GoogleCalendarRepository>();
            services.AddScoped<IGoogleSheetsRepository, GoogleSheetsRepository>();

            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            return services;
        }
    }
}