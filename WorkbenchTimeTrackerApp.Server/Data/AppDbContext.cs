using Microsoft.EntityFrameworkCore;
using WorkbenchTimeTrackerApp.Server.Models;

namespace WorkbenchTimeTrackerApp.Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Person> People { get; set; }
        public DbSet<WorkTask> WorkTasks { get; set; }
        public DbSet<TimeEntry> TimeEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure table names
            modelBuilder.Entity<Person>().ToTable("People");
            modelBuilder.Entity<WorkTask>().ToTable("WorkTasks");
            modelBuilder.Entity<TimeEntry>().ToTable("TimeEntries");

            // Configure one-to-many relationship between WorkTask and TimeEntry
            modelBuilder.Entity<WorkTask>()
                .HasMany(wt => wt.TimeEntries) // A WorkTask has many TimeEntries
                .WithOne(te => te.WorkTask)    // Each TimeEntry is associated with one WorkTask
                .HasForeignKey(te => te.WorkTaskId) // Foreign key in TimeEntry
                .OnDelete(DeleteBehavior.Restrict); // Restrict deletion if related TimeEntries exist

            // Configure one-to-many relationship between Person and TimeEntry
            modelBuilder.Entity<Person>()
                .HasMany(p => p.TimeEntries) // A Person has many TimeEntries
                .WithOne(te => te.Person)    // Each TimeEntry is associated with one Person
                .HasForeignKey(te => te.PersonId) // Foreign key in TimeEntry
                .OnDelete(DeleteBehavior.Restrict); // Restrict deletion if related TimeEntries exist
        }
    }
}