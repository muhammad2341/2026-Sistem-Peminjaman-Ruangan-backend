using Microsoft.EntityFrameworkCore;
using RoomBooking.Api.Models;

namespace RoomBooking.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Room entity
            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.RoomNumber)
                      .IsRequired()
                      .HasMaxLength(20);

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.HasIndex(e => e.RoomNumber)
                      .IsUnique();

                entity.HasQueryFilter(e => !e.IsDeleted); // Soft delete filter
            });

            // Configure Booking entity
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.BookerName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.BookerEmail)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Purpose)
                      .IsRequired()
                      .HasMaxLength(500);

                entity.HasOne(e => e.Room)
                      .WithMany(r => r.Bookings)
                      .HasForeignKey(e => e.RoomId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasQueryFilter(e => !e.IsDeleted); // Soft delete filter
            });
        }
    }
}
