using users_api.DTO;
using users_api.Models;

namespace users_api.MappingExtensions
{
    public static class MappingExtensions
    {
        public static UserApiDTO toDTO(this User user) => new
            (user.Id, user.Email, user.Password, user?.Notes.Select(n => n.toDTO()));
        public static NoteApiDTO toDTO(this Note note) => new
            (note.Id, note.Content);

    }
}
