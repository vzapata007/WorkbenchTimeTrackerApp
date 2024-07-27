using System;
using System.ComponentModel.DataAnnotations;

namespace WorkbenchTimeTrackerApp.Server.Models
{
    public class TimeEntry
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Date and time are required.")]
        public DateTime EntryDateTime { get; set; }

        // Navigation properties
        public int PersonId { get; set; }
        public Person? Person { get; set; }
        public int WorkTaskId { get; set; }
        public WorkTask? WorkTask { get; set; }
    }
}