using Application.Commands.MedicationCommand;
using Application.DTOs;
using Application.Queries.MedicationQueries;
using Application.Use_Cases.Queries.MedicationQueries;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PHMS.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MedicationController : ControllerBase
    {
        private readonly IMediator mediator;
        public MedicationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Result<Guid>>> CreateMedication(CreateMedicationCommand command)
        {
            try
            {
                var result = await mediator.Send(command);
                return CreatedAtAction("GetByID", new { Id = result.Data }, result.Data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByID(Guid id)
        {
            try
            {
                var result = await mediator.Send(new GetMedicationByIdQuery { MedicationId = id });
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
        public async Task<ActionResult<IEnumerable<MedicationDto>>> GetAll()
        {
            try
            {
                var medications = await mediator.Send(new GetAllMedicationsQuery());
                return Ok(medications);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateMedicationCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("The id should be identical with command.MedicationId");
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
            try
            {
                var result = await mediator.Send(new DeleteMedicationByIdCommand(id));
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

        [HttpGet("external/{condition}")]
        public async Task<IActionResult> GetMedicationsFromExternalSource(string condition)
        {
            try
            {
                var result = await mediator.Send(new GetExternalMedicationByConditionQuery { Condition = condition });
                return Ok(result);
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}