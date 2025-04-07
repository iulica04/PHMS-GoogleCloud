using Application.DTOs;
using Application.Use_Cases.Commands.ConsultationCommands;
using Application.Use_Cases.Queries.ConsultationsQueries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PHMS.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class ConsultationController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;

        public ConsultationController(IMediator mediator, IConfiguration configuration)
        {
            this.mediator = mediator;
            this.configuration = configuration;
        }

        [HttpPost("request")]
        public async Task<IActionResult> RequestConsultation([FromBody] CreateConsultationCommand command)
        {
            var result = await mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(new { ConsultationId = result.Data });
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConsultationDto>>> GetAll()
        {
            var result = await mediator.Send(new GetAllConsultationsQuery());
            return Ok(result);
        }
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByID(Guid id)
        {
            var result = await mediator.Send(new GetConsultationByIdQuery { Id= id });
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return NotFound(result.ErrorMessage);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateConsultationCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("The id should be identical with command.ConsultationId");
            }

            var result = await mediator.Send(command);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return NotFound(result.ErrorMessage);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await mediator.Send(new DeleteConsultationCommand(id));
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return NotFound(result.ErrorMessage);
        }

  


    }
}
