using Microsoft.EntityFrameworkCore;
using users_api.Models;

namespace users_api
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }

    }
}
