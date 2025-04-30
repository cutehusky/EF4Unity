using Microsoft.EntityFrameworkCore;

namespace Sample.Scripts
{
    public class PlayerDBContext: DbContext
    {
        public DbSet<Player> Player { get; set; }

        public PlayerDBContext() : base() { }
        public PlayerDBContext(DbContextOptions<PlayerDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Player>();
        }
    }
}