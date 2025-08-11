using Microsoft.EntityFrameworkCore;
using users_api.DTO;
using users_api.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace users_api.DBRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context;
        public UserRepository(UserContext context)
        {
            _context = context;
        }
        public async Task<User?> GetUserByIdAsync(Guid? userId)
        {
            if (userId == null || userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be null or empty", nameof(userId));
        
            return await _context.Users
                .Include(user => user.Notes)
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.Id == userId);
        }
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            if (email is null)
                throw new ArgumentNullException(nameof(email));

            return await _context.Users
                .Include(user => user.Notes)
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.Email == email);
        }
        public async Task<Guid> AddUserAsync(UserCreateDTO userCreateDTO)
        {
            UserCreateDTO.Check(userCreateDTO);

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (await _context.Users.AnyAsync(u => u.Email == userCreateDTO.Email))
                    throw new InvalidOperationException("Email already exists");

                var newUser = new User()
                {
                    Email = userCreateDTO.Email,
                    Password = PasswordHelper.HashPassword(userCreateDTO.Password)
                };
                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return newUser.Id;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }            
        }
        public async Task UpdateUserInfoAsync(UserUpdateDTO updateUserDTO)
        {
            UserUpdateDTO.Check(updateUserDTO);

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (!await _context.Users.AnyAsync(u => u.Id == updateUserDTO.id))
                    throw new InvalidOperationException("User not found");
                if (await _context.Users.AnyAsync(u =>
                    u.Email == updateUserDTO.Email &&
                    u.Id != updateUserDTO.id))
                    throw new InvalidOperationException("Email already in use");
                
                if (updateUserDTO.Password == null)
                    await _context.Users
                    .Where(u => u.Id == updateUserDTO.id)
                        .ExecuteUpdateAsync(setters => setters
                        .SetProperty(u => u.Email, updateUserDTO.Email));
                else await _context.Users
                    .Where(u => u.Id == updateUserDTO.id)
                        .ExecuteUpdateAsync(setters => setters
                        .SetProperty(u => u.Email, updateUserDTO.Email)
                        .SetProperty(u => u.Password, PasswordHelper.HashPassword(updateUserDTO.Password)));
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
            
        }
        public async Task DeleteUserByIdAsync(Guid? userId)
        {
            if (userId == null || userId == Guid.Empty)
                throw new ArgumentException("Invalid user ID", nameof(userId));

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Notes
                    .Where(n => n.User.Id == userId)
                    .ExecuteDeleteAsync();

                var deletedCount = await _context.Users
                    .Where(u => u.Id == userId)
                    .ExecuteDeleteAsync();

                if (deletedCount == 0)
                    throw new InvalidOperationException($"user with id {userId} not found");

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task DeleteUserByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException(nameof(email));
            
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Notes
                .Where(n => n.User.Email == email)
                .ExecuteDeleteAsync();

                var deletedCount = await _context.Users
                    .Where(u => u.Email == email)
                    .ExecuteDeleteAsync();

                if (deletedCount == 0)
                    throw new InvalidOperationException($"user with email {email} not found");

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
