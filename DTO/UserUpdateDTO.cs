namespace users_api.DTO
{
    public record UserUpdateDTO(Guid? id, string? Password, string? Email)
    {
        public static void Check(UserUpdateDTO userUpdateDTO)
        {
            if (userUpdateDTO == null)
                throw new ArgumentNullException(nameof(userUpdateDTO));
            
            if (userUpdateDTO.id == null)
                throw new ArgumentNullException();
            
            
        }
    }
}
