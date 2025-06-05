using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repository
{
    public class DBCodeLockerContext : DbContext
    {
        public DBCodeLockerContext(DbContextOptions<DBCodeLockerContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> usuario { get; set; }
        public DbSet<Salas> Salas { get; set; }
        public DbSet<Tipo> Tipos { get; set; }
        public DbSet<Status> Status { get; set; }
    }
}
