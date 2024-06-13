using Microsoft.EntityFrameworkCore;
using Modele;

namespace WymianaWaluty.Modele
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<CurrencyBalance> CurrencyBalances { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Balance)
                      .HasColumnType("decimal(18,2)")
                      .HasDefaultValue(0);

                entity.HasMany(e => e.CurrencyBalances)
                      .WithOne(e => e.User)
                      .HasForeignKey(e => e.UserId);
            });
        }
    }
}