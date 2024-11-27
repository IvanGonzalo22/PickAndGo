using Microsoft.EntityFrameworkCore;
using PickAndGo.Server.Models;

namespace PickAndGo.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }  // Aquí se cambia a 'Users' si estamos usando una única tabla
    }
}
