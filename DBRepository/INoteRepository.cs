using users_api.DTO;
using users_api.Models;

namespace users_api.DBRepository
{
    public interface INoteRepository
    {
        public Task<IEnumerable<Note>> GetNotesByUserIdAsync (Guid userId);
        public Task<Note> GetNoteByIdAsync (Guid id);
        public Task<Guid> AddNoteAsync(NoteCreateDTO noteCreateDTO);
        public Task UpdateNoteAsync(NoteUpdateDTO newNote);
        public Task DeleteNoteByIdAsync (Guid id);
    }
}
