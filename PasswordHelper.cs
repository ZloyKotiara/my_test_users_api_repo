using Microsoft.AspNetCore.Identity;

namespace users_api
{
    public class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            var hasher = new PasswordHasher<object>();
            return hasher.HashPassword(null, password);
        }
    }
}
