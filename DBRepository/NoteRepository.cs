using Microsoft.EntityFrameworkCore;
using users_api.DTO;
using users_api.Models;

namespace users_api.DBRepository
{
    public class NoteRepository : INoteRepository
    {
        private readonly UserContext _context;
        public NoteRepository(UserContext userContext)
        {
            _context = userContext;
        }
        public async Task<IEnumerable<Note>> GetNotesByUserIdAsync(Guid userId)
        { 
            if (userId == null || userId == Guid.Empty)
                throw new ArgumentNullException("User ID cannot be null or empty", nameof(userId));

            return await _context.Notes
                .Where(n => n.User.Id == userId)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<Note> GetNoteByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException();

            return await _context.Notes
                .Where(n => n.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
        public async Task<Guid> AddNoteAsync(NoteCreateDTO noteCreateDTO)
        {
            NoteCreateDTO.Check(noteCreateDTO);

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = await _context.Users.FindAsync(noteCreateDTO.UserId);
                if (user == null)
                    throw new InvalidOperationException($"User with ID {noteCreateDTO.UserId} not found");

                var newNote = new Note()
                {
                    Content = noteCreateDTO.Content,
                    User = user
                };
                await _context.Notes.AddAsync(newNote);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return newNote.Id;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task DeleteNoteByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Note ID cannot be empty", nameof(id));
            
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try 
            {
                var deletedCount = await _context.Notes.
                    Where(n => n.Id == id).
                    ExecuteDeleteAsync();
                if (deletedCount == 0)
                    throw new KeyNotFoundException($"note with if {id} not found");
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
