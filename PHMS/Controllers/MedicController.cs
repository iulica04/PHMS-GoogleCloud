using Application.Commands.Medic;
using Application.DTOs;
using Application.Queries;
using Application.Queries.MedicQueries;
using Application.Use_Cases.Authentification;
using Application.Utils;
using Domain.Common;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace PHMS.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MedicController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly string JWT_SECRET;

        public MedicController(IMediator mediator, IConfiguration configuration)
        {
            this.mediator = mediator;
            JWT_SECRET = configuration["Jwt:Key"]!;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMedic(CreateMedicCommand command)
        {
            try
            {
                var id = await mediator.Send(command);
                return CreatedAtAction("GetByID", new { Id = id.Data }, id.Data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<Result<LoginResponse>>> LoginMedic(LoginUserCommand command)
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
        public async Task<ActionResult<List<MedicDto>>> GetAllMedics()
        {
            try
            {
                return await mediator.Send(new GetAllMedicsQuery());
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
                var result = await mediator.Send(new GetMedicByIdQuery { Id = id });
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMedic(Guid id, UpdateMedicCommand command)
        {
            var authHeader = Request.Headers.Authorization.ToString();
            var authStatus = IAuthorizationManager.EnsureProperAuthorization(authHeader, JWT_SECRET, id, ["Admin"]);
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
        public async Task<IActionResult> DeleteMedic(Guid id)
        {
            var authHeader = Request.Headers.Authorization.ToString();
            var authStatus = IAuthorizationManager.EnsureProperAuthorization(authHeader, JWT_SECRET, id, ["Admin"]);
            if (!authStatus.IsSuccess)
            {
                return Unauthorized(authStatus.ErrorMessage);
            }

            try
            {
                var result = await mediator.Send(new DeleteMedicByIdCommand(id));
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

        [HttpGet("paginated")]
        public async Task<ActionResult<PagedResult<MedicDto>>> GetPaginatedMedics([FromQuery] int page,
            [FromQuery] int pageSize, [FromQuery] string? rank, [FromQuery] string? specialization)
        {
            Expression<Func<Medic, bool>> filter = m =>
               (string.IsNullOrEmpty(rank) || m.Rank == rank) &&
               (string.IsNullOrEmpty(specialization) || m.Specialization == specialization);

            var query = new GetFilteredQuery<Medic, MedicDto>
            {
                Page = page,
                PageSize = pageSize,
                Filter = filter
            };

            try
            {
                var result = await mediator.Send(query);
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
    }
}