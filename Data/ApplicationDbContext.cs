using Firma_tootajate_registreerimissusteem.Models;
using Microsoft.EntityFrameworkCore;

namespace Firma_tootajate_registreerimissusteem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Register> Registers { get; set; }
        public DbSet<Login> Logins { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}