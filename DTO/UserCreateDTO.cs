namespace users_api.DTO
{
    public record UserCreateDTO(string? Password, string? Email)
    {
        public static void Check(UserCreateDTO userCreateDTO)
        {
            if (userCreateDTO == null)
                throw new ArgumentNullException(nameof(userCreateDTO));
            
            if (userCreateDTO.Password == null || userCreateDTO.Password == "")
                throw new ArgumentNullException(nameof(userCreateDTO));

            if (userCreateDTO.Email == null || userCreateDTO.Password == "")
                throw new ArgumentNullException(nameof(userCreateDTO));
            
        }
    }
}
