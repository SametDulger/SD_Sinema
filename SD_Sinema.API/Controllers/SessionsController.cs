using Microsoft.AspNetCore.Mvc;
using SD_Sinema.Business.DTOs;
using SD_Sinema.Business.Services;

namespace SD_Sinema.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionsController : ControllerBase
    {
        private readonly ISessionService _sessionService;

        public SessionsController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SessionDto>>> GetAll()
        {
            var sessions = await _sessionService.GetAllAsync();
            return Ok(sessions);
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<SessionDto>>> GetActive()
        {
            var sessions = await _sessionService.GetActiveSessionsAsync();
            return Ok(sessions);
        }

        [HttpGet("movie/{movieId}")]
        public async Task<ActionResult<IEnumerable<SessionDto>>> GetByMovie(int movieId)
        {
            var sessions = await _sessionService.GetSessionsByMovieAsync(movieId);
            return Ok(sessions);
        }

        [HttpGet("salon/{salonId}")]
        public async Task<ActionResult<IEnumerable<SessionDto>>> GetBySalon(int salonId)
        {
            var sessions = await _sessionService.GetSessionsBySalonAsync(salonId);
            return Ok(sessions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SessionDto>> GetById(int id)
        {
            var session = await _sessionService.GetByIdAsync(id);
            if (session == null)
                return NotFound();

            return Ok(session);
        }

        [HttpPost]
        public async Task<ActionResult<SessionDto>> Create(CreateSessionDto createSessionDto)
        {
            try
            {
                var session = await _sessionService.CreateAsync(createSessionDto);
                return CreatedAtAction(nameof(GetById), new { id = session.Id }, session);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SessionDto>> Update(int id, UpdateSessionDto updateSessionDto)
        {
            try
            {
                var session = await _sessionService.UpdateAsync(id, updateSessionDto);
                return Ok(session);
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
                await _sessionService.DeleteAsync(id, deletedBy, reason);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
} 