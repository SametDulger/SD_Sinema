using Microsoft.AspNetCore.Mvc;
using SD_Sinema.Business.DTOs;
using SD_Sinema.Business.Services;

namespace SD_Sinema.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalonsController : ControllerBase
    {
        private readonly ISalonService _salonService;

        public SalonsController(ISalonService salonService)
        {
            _salonService = salonService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalonDto>>> GetAll()
        {
            var salons = await _salonService.GetAllAsync();
            return Ok(salons);
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<SalonDto>>> GetActive()
        {
            var salons = await _salonService.GetActiveSalonsAsync();
            return Ok(salons);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SalonDto>> GetById(int id)
        {
            var salon = await _salonService.GetByIdAsync(id);
            if (salon == null)
                return NotFound();

            return Ok(salon);
        }

        [HttpPost]
        public async Task<ActionResult<SalonDto>> Create(CreateSalonDto createSalonDto)
        {
            try
            {
                var salon = await _salonService.CreateAsync(createSalonDto);
                return CreatedAtAction(nameof(GetById), new { id = salon.Id }, salon);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SalonDto>> Update(int id, UpdateSalonDto updateSalonDto)
        {
            try
            {
                var salon = await _salonService.UpdateAsync(id, updateSalonDto);
                return Ok(salon);
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
                await _salonService.DeleteAsync(id, deletedBy, reason);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
} 