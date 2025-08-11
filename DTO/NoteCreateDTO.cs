namespace users_api.DTO
{
    public record NoteCreateDTO(string? Content, Guid? UserId) 
    {
        public static void Check(NoteCreateDTO noteCreateDTO)
        {
            if (noteCreateDTO == null)            
                throw new ArgumentNullException(nameof(noteCreateDTO));
            
            if (noteCreateDTO.Content == null)           
                throw new ArgumentNullException(nameof(noteCreateDTO));
            
            if (noteCreateDTO.UserId == null)            
                throw new ArgumentNullException(nameof(noteCreateDTO));
            
        }
    }
}
