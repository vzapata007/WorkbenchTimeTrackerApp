using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkbenchTimeTrackerApp.Server.Models
{
    public class Person
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, ErrorMessage = "Full name can't be longer than 100 characters.")]
        public required string FullName { get; set; }

        // One-to-many relationship with TimeEntry
        public ICollection<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();

    }
}