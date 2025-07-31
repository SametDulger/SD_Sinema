using Microsoft.AspNetCore.Mvc;
using SD_Sinema.Business.DTOs;
using SD_Sinema.Business.Services;
using SD_Sinema.Core.Entities;
using SD_Sinema.Core.Interfaces;

namespace SD_Sinema.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly IGenreService _genreService;

        public MoviesController(IMovieService movieService, IGenreService genreService)
        {
            _movieService = movieService;
            _genreService = genreService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetAll()
        {
            var movies = await _movieService.GetAllAsync();
            return Ok(movies);
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetActive()
        {
            var movies = await _movieService.GetActiveMoviesAsync();
            return Ok(movies);
        }

        [HttpGet("genres")]
        public async Task<ActionResult<IEnumerable<string>>> GetGenres()
        {
            var genreNames = await _genreService.GetActiveGenreNamesAsync();
            return Ok(genreNames);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDto>> GetById(int id)
        {
            var movie = await _movieService.GetByIdAsync(id);
            if (movie == null)
                return NotFound();

            return Ok(movie);
        }

        [HttpPost]
        public async Task<ActionResult<MovieDto>> Create(CreateMovieDto createMovieDto)
        {
            if (createMovieDto == null)
                return BadRequest("Film bilgileri boş olamaz.");

            try
            {
                var movie = await _movieService.CreateAsync(createMovieDto);
                return CreatedAtAction(nameof(GetById), new { id = movie.Id }, movie);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MovieDto>> Update(int id, UpdateMovieDto updateMovieDto)
        {
            if (updateMovieDto == null)
                return BadRequest("Film bilgileri boş olamaz.");

            try
            {
                var movie = await _movieService.UpdateAsync(id, updateMovieDto);
                return Ok(movie);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, [FromQuery] string deletedBy = "System", [FromQuery] string reason = "Manual deletion")
        {
            try
            {
                await _movieService.DeleteAsync(id, deletedBy, reason);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
} 