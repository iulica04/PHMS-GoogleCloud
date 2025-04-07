using Application.Commands.Patient;
using Application.DTOs;
using Application.Queries;
using Application.Queries.PatientQueries;
using Application.Use_Cases.Authentification;
using Application.Use_Cases.Commands.PatientCommands;
using Application.Use_Cases.ResetPassword;
using Domain.Common;
using Domain.Entities;
using Domain.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PHMS.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;
        private readonly IEmailService emailService;
        private readonly IValidationTokenService validationTokenService;

        public PatientController(IMediator mediator, IConfiguration configuration, IEmailService emailService, IValidationTokenService validationTokenService)
        {
            this.mediator = mediator;
            this.configuration = configuration;
            this.emailService = emailService;
            this.validationTokenService = validationTokenService;
        }

        [HttpPost]
        public async Task<ActionResult<Result<Guid>>> CreatePatient(CreatePatientCommand command)
        {
            try
            {
                var result = await mediator.Send(command);
                return CreatedAtAction(nameof(GetByID), new { Id = result.Data }, result.Data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<Result<LoginResponse>>> LoginPatient(LoginUserCommand command)
        {
            var response = await mediator.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response.Data);
            }
            else if(response.ErrorMessage == "No account found with this email address. Please try again or sign up.")
            {
                return NotFound(response.ErrorMessage);
            }
            else 
            {
                return Unauthorized(response.ErrorMessage);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(Guid id)
        {
            var authHeader = Request.Headers.Authorization.ToString();
            var authStatus = IAuthorizationManager.EnsureProperAuthorization(authHeader, configuration["Jwt:Key"]!, id, ["Medic", "Admin"]);
            if (!authStatus.IsSuccess)
            {
                return Unauthorized(authStatus.ErrorMessage);
            }

            var result = await mediator.Send(new GetPatientByIdQuery { Id = id });
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return NotFound(result.ErrorMessage);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetAll()
        {
            try
            {
                var patients = await mediator.Send(new GetAllPatientsQuery());
                return Ok(patients);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdatePatientCommand command)
        {
            var authHeader = Request.Headers.Authorization.ToString();
            var authStatus = IAuthorizationManager.EnsureProperAuthorization(authHeader, configuration["Jwt:Key"]!, id, ["Admin"]);
            if (!authStatus.IsSuccess)
            {
                return Unauthorized(authStatus.ErrorMessage);
            }

            if (id != command.Id)
            {
                return BadRequest("The id should be identical with command.Id");
            }

            try
            {
                var result = await mediator.Send(command);
                if (result.IsSuccess)
                {
                    return NoContent();
                }
                return NotFound(result.ErrorMessage);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var authHeader = Request.Headers.Authorization.ToString();
            var authStatus = IAuthorizationManager.EnsureProperAuthorization(authHeader, configuration["Jwt:Key"]!, id, ["Admin"]);
            if (!authStatus.IsSuccess)
            {
                return Unauthorized(authStatus.ErrorMessage);
            }

            try
            {
                var result = await mediator.Send(new DeletePatientByIdCommand(id));
                if (result.IsSuccess)
                {
                    return NoContent();
                }
                return NotFound(result.ErrorMessage);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("check-email")]
        public async Task<IActionResult> CheckEmail(string email)
        {
            try
            {
                var exists = await mediator.Send(new CheckEmailQuery { Email = email });
                return Ok(new { exists });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { success = false, message = "Email is required" });
            }

            try
            {
                var token = await validationTokenService.GenerateResetTokenAsync(email);
                var resetLink = $"http://localhost:4200/reset-password/{token}"; // Construiește URL-ul manual

                var message = $"Click the link to reset your password: {resetLink}";
                await emailService.SendEmailAsync(email, "Password Reset", message);
                return Ok(new { success = true, message = "Verification link sent to your email" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
        {
            try
            {
                var result = await mediator.Send(command);
                if (result.IsSuccess)
                {
                    return Ok(new { success = true, message = "Password reset successfully" });
                }
                return BadRequest(new { success = false, message = result.ErrorMessage });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:guid}/update-password")]
        public async Task<IActionResult> UpdatePassword(Guid id, UpdatePatientPasswordCommand command)
        {
            var authHeader = Request.Headers.Authorization.ToString();
            var authStatus = IAuthorizationManager.EnsureProperAuthorization(authHeader, configuration["Jwt:Key"]!, id, ["Admin"]);
            if (!authStatus.IsSuccess)
            {
                return Unauthorized(authStatus.ErrorMessage);
            }

            Console.WriteLine($"id: {id}, command.PatientId: {command.PatientId}");

            if (id != command.PatientId)
            {
                return BadRequest("The id should be identical with command.Id");
            }

            try
            {
                var result = await mediator.Send(command);
                if (result.IsSuccess)
                {
                    return NoContent();
                }
                return BadRequest(result.ErrorMessage);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportPatients()
        {
            var link = await mediator.Send(new ExportPatientsCommand());
            return Ok(new { sheetLink = link });
        }
    }
}
