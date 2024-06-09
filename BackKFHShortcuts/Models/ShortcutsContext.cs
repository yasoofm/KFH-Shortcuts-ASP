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
        public DbSet<SystemMessage> SystemMessages { get; set; }

        public ShortcutsContext(DbContextOptions<ShortcutsContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasData(new List<User>
            {
                User.Create(1, "admin@mail.com", "1234", "Yousef", "Mandani", 0, 1111, true),
                User.Create(2, "user@mail.com", "1234", "Yousef", "Mandani", 0, 1234),
            });
            modelBuilder.Entity<SystemMessage>().HasData(new List<SystemMessage>
            {
                new SystemMessage
                {
                    Id = 1,
                    Role = "system",
                    Content = "If asked about Prepaid Cards reply with Oasis Club Mastercard World, Hesabi, Al-Osra, and Foreign Currency",
                },
                new SystemMessage
                {
                    Id = 2,
                    Role = "system",
                    Content = "If asked about Financial Investments reply with Al Nuwair, Al Sidra, Al Dima, and Al Khomasiyah"
                },
                new SystemMessage
                {
                    Id = 3,
                    Role = "system",
                    Content = "If asked about Accounts reply with Alrabeh and Investment saving account"
                },
                new SystemMessage
                {
                    Id = 4,
                    Role = "system",
                    Content = "If asked about Credit Cards reply with Oasis Club Mastercard World and Visa Diamond"
                },
                new SystemMessage
                {
                    Id = 5,
                    Role = "system",
                    Content = "You are a chatbot used to help KFH Employees promote and sell KFH products"
                },
                new SystemMessage
                {
                    Id = 6,
                    Role = "system",
                    Content = "make your replies as short and concise as possible while giving the asked for information in a clear way"
                },
            });
        }
    }
}
