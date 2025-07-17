using Microsoft.AspNetCore.Mvc;
using SD_Sinema.Business.DTOs;
using SD_Sinema.Business.Services;

namespace SD_Sinema.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketTypesController : ControllerBase
    {
        private readonly ITicketTypeService _ticketTypeService;

        public TicketTypesController(ITicketTypeService ticketTypeService)
        {
            _ticketTypeService = ticketTypeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketTypeDto>>> GetAll()
        {
            var ticketTypes = await _ticketTypeService.GetAllAsync();
            return Ok(ticketTypes);
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<TicketTypeDto>>> GetActive()
        {
            var ticketTypes = await _ticketTypeService.GetActiveTicketTypesAsync();
            return Ok(ticketTypes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TicketTypeDto>> GetById(int id)
        {
            var ticketType = await _ticketTypeService.GetByIdAsync(id);
            if (ticketType == null)
                return NotFound();

            return Ok(ticketType);
        }

        [HttpPost]
        public async Task<ActionResult<TicketTypeDto>> Create(CreateTicketTypeDto createTicketTypeDto)
        {
            try
            {
                var ticketType = await _ticketTypeService.CreateAsync(createTicketTypeDto);
                return CreatedAtAction(nameof(GetById), new { id = ticketType.Id }, ticketType);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TicketTypeDto>> Update(int id, UpdateTicketTypeDto updateTicketTypeDto)
        {
            try
            {
                var ticketType = await _ticketTypeService.UpdateAsync(id, updateTicketTypeDto);
                return Ok(ticketType);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, [FromQuery] string deletedBy, [FromQuery] string reason)
        {
            try
            {
                await _ticketTypeService.DeleteAsync(id, deletedBy, reason);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
} 