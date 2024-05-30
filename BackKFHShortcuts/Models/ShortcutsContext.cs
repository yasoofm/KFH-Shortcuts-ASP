using BackKFHShortcuts.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackKFHShortcuts.Models
{
    public class ShortcutsContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductRequest> ProductRequests { get; set; }
        public DbSet<Reward> Rewards { get; set; }
        public DbSet<RewardRequest> RewardRequests { get; set; }
        public DbSet<Message> Messages { get; set; }

        public ShortcutsContext(DbContextOptions<ShortcutsContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasData(new List<User>
            {
                User.Create(1, "admin@mail.com", "1234", "Yousef", "Mandani", 0, 1111, true),
                User.Create(2, "user@mail.com", "1234", "Yousef", "Mandani", 0, 1234),
            });
        }
    }
}
