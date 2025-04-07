using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Commands.Administrator;
using Application.Queries.AdminQueries;
using Application.Use_Cases.Authentification;
using Domain.Entities;
using Domain.Common;

namespace PHMS.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;

        public AdminController(IMediator mediator, IConfiguration configuration)
        {
            this.mediator = mediator;
            this.configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<ActionResult<Result<LoginResponse>>> LoginAdmin(LoginUserCommand command)
        {
            var response = await mediator.Send(command);
            if (response.IsSuccess)
            {
                return Ok(response.Data);
            }
            else if (response.ErrorMessage == "No account found with this email address. Please try again or sign up.")
            {
                return NotFound(response.ErrorMessage);
            }
            else
            {
                return Unauthorized(response.ErrorMessage);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminDto>>> GetAll()
        {
            try
            {
                var admins = await mediator.Send(new GetAllAdminsQuery());
                return Ok(admins);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(Guid id)
        {
            try
            {
                var result = await mediator.Send(new GetAdminByIdQuery { Id = id });
                if (result.IsSuccess)
                {
                    return Ok(result.Data);
                }
                return NotFound(result.ErrorMessage);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateAdminCommand command)
        {
            var authHeader = Request.Headers.Authorization.ToString();
            var authStatus = IAuthorizationManager.EnsureProperAuthorization(authHeader, configuration["Jwt:Key"]!, id, []);
            if (!authStatus.IsSuccess)
            {
                return Unauthorized(authStatus.ErrorMessage);
            }

            if (id != command.Id)
            {
                return BadRequest();
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var authHeader = Request.Headers.Authorization.ToString();
            var authStatus = IAuthorizationManager.EnsureProperAuthorization(authHeader, configuration["Jwt:Key"]!, id, []);
            if (!authStatus.IsSuccess)
            {
                return Unauthorized(authStatus.ErrorMessage);
            }

            try
            {
                var result = await mediator.Send(new DeleteAdminByIdCommand(id));
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
    }
}
