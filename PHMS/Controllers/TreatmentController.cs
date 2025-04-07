using Application.Commands.TreatmentCommands;
using Application.DTOs;
using Application.Queries.TreatmentQueries;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PHMS.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TreatmentController : ControllerBase
    {
        private readonly IMediator mediator;
        public TreatmentController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Result<Guid>>> CreateTreatment(CreateTreatmentCommand command)
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(Guid id)
        {
            try
            {
                var result = await mediator.Send(new GetTreatmentByIdQuery { TreatmentId = id });
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TreatmentDto>>> GetAll()
        {
            try
            {
                var treatments = await mediator.Send(new GetAllTreatmentsQuery());
                return Ok(treatments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateTreatmentCommand command)
        {
            if (id != command.TreatmentId)
            {
                return BadRequest("The id should be identical with command.TreatmentId");
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

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await mediator.Send(new DeleteTreatmentByIdCommand(id));
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
    }
}
