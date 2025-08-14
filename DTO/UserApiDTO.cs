
namespace users_api.DTO
{
    public record UserApiDTO(Guid Id, string Email, string Password, IEnumerable<NoteApiDTO>? notes)
    {}
}
