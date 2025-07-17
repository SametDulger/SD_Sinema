using Microsoft.AspNetCore.Mvc;
using SD_Sinema.Business.DTOs;
using SD_Sinema.Business.Services;

namespace SD_Sinema.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketDto>>> GetAll()
        {
            var tickets = await _ticketService.GetAllAsync();
            return Ok(tickets);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<TicketDto>>> GetByUser(int userId)
        {
            var tickets = await _ticketService.GetTicketsByUserAsync(userId);
            return Ok(tickets);
        }

        [HttpGet("session/{sessionId}")]
        public async Task<ActionResult<IEnumerable<TicketDto>>> GetBySession(int sessionId)
        {
            var tickets = await _ticketService.GetTicketsBySessionAsync(sessionId);
            return Ok(tickets);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TicketDto>> GetById(int id)
        {
            var ticket = await _ticketService.GetByIdAsync(id);
            if (ticket == null)
                return NotFound();

            return Ok(ticket);
        }

        [HttpPost]
        public async Task<ActionResult<TicketDto>> Create(CreateTicketDto createTicketDto)
        {
            try
            {
                var ticket = await _ticketService.CreateAsync(createTicketDto);
                return CreatedAtAction(nameof(GetById), new { id = ticket.Id }, ticket);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TicketDto>> Update(int id, UpdateTicketDto updateTicketDto)
        {
            try
            {
                var ticket = await _ticketService.UpdateAsync(id, updateTicketDto);
                return Ok(ticket);
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
                await _ticketService.DeleteAsync(id, deletedBy, reason);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("calculate-price")]
        public async Task<ActionResult<decimal>> CalculatePrice([FromQuery] int ticketTypeId, [FromQuery] int userId)
        {
            try
            {
                var price = await _ticketService.CalculateTicketPriceAsync(ticketTypeId, userId);
                return Ok(price);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
} 