namespace users_api.DTO
{
    public record NoteUpdateDTO(string? Content, Guid? NoteId)
    {
        public static void Check(NoteUpdateDTO noteUpdateDTO)
        {
            if (noteUpdateDTO == null)
                throw new ArgumentNullException(nameof(noteUpdateDTO));

            if (noteUpdateDTO.Content == null)
                throw new ArgumentNullException(nameof(noteUpdateDTO));

            if (noteUpdateDTO.NoteId == null)
                throw new ArgumentNullException(nameof(noteUpdateDTO));

        }
    }
}
