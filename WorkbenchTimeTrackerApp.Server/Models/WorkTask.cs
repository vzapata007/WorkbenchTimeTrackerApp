using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkbenchTimeTrackerApp.Server.Models
{
    public class WorkTask
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Task name is required.")]
        [StringLength(100, ErrorMessage = "Task name can't be longer than 100 characters.")]
        public required string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public required string Description { get; set; }

        // Navigation property for TimeEntries
        public ICollection<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();
    }
}