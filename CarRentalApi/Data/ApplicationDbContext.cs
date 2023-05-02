using CarRentalApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApi.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }
        
        public DbSet<User> Users { get; set; }

        public DbSet <Car> Cars { get; set; }

        public DbSet <Booking> Bookings { get; set; }

        public DbSet <Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(user => user.Email).IsUnique();
            modelBuilder.Entity<User>().HasIndex(user => user.PhoneNumber).IsUnique();
            modelBuilder.Entity<Car>().HasIndex(car => car.RegNumber).IsUnique();

          /*  modelBuilder.Entity<Booking>()
                .HasOne(b => b.BookedBy)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.BookedBy)
                .OnDelete(DeleteBehavior.NoAction);*/
           
        }
    }
}
