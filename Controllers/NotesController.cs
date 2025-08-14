using Microsoft.AspNetCore.Mvc;
using users_api.DBRepository;
using users_api.DTO;
using users_api.Models;

namespace users_api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Produces("application/json")]    
    public class NotesController : ControllerBase
    {
        private readonly INoteRepository _noteRepository;
        private readonly ILogger<NotesController> _logger;
        public NotesController(INoteRepository noteRepository, ILogger<NotesController> logger)
        {
            _noteRepository = noteRepository;
            _logger = logger;
        }

        [HttpGet("user/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Note>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetNotesByUserIdAsync(Guid id)
        {
            try
            {
                var notes = await _noteRepository.GetNotesByUserIdAsync(id);
                return notes == null ? NotFound() : Ok(notes);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error getting notes with userID {id}");
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Note))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetNoteByIdAsync(Guid id)
        {
            try
            {
                var note = await _noteRepository.GetNoteByIdAsync(id);
                return note == null ? NotFound() : Ok(note);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error getting note by id {id}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Guid))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostNoteByUserId(NoteCreateDTO noteCreateDTO)
        {
            try
            {
                var id = await _noteRepository.AddNoteAsync(noteCreateDTO);
                return StatusCode(201, id);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error creating note by Userid");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> NoteUpdateAsync(NoteUpdateDTO noteUpdateDTO)
        {
            try
            {
                await _noteRepository.UpdateNoteAsync(noteUpdateDTO);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error updating note");
                return BadRequest("Internal server error");
            }
        }
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteNoteById (Guid id)
        {
            try
            {
                await _noteRepository.DeleteNoteByIdAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex) 
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error deleting note");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
