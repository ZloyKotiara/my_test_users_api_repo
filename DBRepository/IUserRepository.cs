using users_api.DTO;
using users_api.Models;

namespace users_api.DBRepository
{
    public interface IUserRepository
    {
        public Task<User?> GetUserByIdAsync(Guid? userId);
        public Task<IEnumerable<User>> GetUsersAsync();
        public Task<User?> GetUserByEmailAsync(string email);
        public Task<Guid> AddUserAsync(UserCreateDTO userCreateDTO);
        public Task UpdateUserInfoAsync(UserUpdateDTO updateUserDto);
        public Task DeleteUserByIdAsync(Guid? userId);
        public Task DeleteUserByEmailAsync(string email);        
    }
}
