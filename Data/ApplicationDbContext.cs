using Eventsystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Eventsystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingItem> BookingItems { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Event relationships
            modelBuilder.Entity<Event>()
                .HasOne(e => e.Category)
                .WithMany(c => c.Events)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.Venue)
                .WithMany(v => v.Events)
                .HasForeignKey(e => e.VenueId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.Organizer)
                .WithMany(u => u.Events)
                .HasForeignKey(e => e.OrganizerId)
                .OnDelete(DeleteBehavior.Restrict);

            // TicketType
            modelBuilder.Entity<TicketType>()
                .HasOne(t => t.Event)
                .WithMany(e => e.TicketTypes)
                .HasForeignKey(t => t.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            // Booking
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasIndex(b => b.BookingReference)
                .IsUnique();

            // BookingItem
            modelBuilder.Entity<BookingItem>()
                .HasOne(i => i.Booking)
                .WithMany(b => b.Items)
                .HasForeignKey(i => i.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BookingItem>()
                .HasOne(i => i.TicketType)
                .WithMany(t => t.BookingItems)
                .HasForeignKey(i => i.TicketTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Review
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Event)
                .WithMany(e => e.Reviews)
                .HasForeignKey(r => r.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
