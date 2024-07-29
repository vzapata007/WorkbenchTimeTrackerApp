using System;
using System.ComponentModel.DataAnnotations;

namespace WorkbenchTimeTrackerApp.Server.DTOs
{
    public class TimeEntryDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "EntryDateTime is required.")]
        public DateTime EntryDateTime { get; set; }

        [Required(ErrorMessage = "PersonId is required.")]
        public int PersonId { get; set; }
        public string? PersonFullName { get; set; }

        [Required(ErrorMessage = "WorkTaskId is required.")]
        public int WorkTaskId { get; set; }
        public string? WorkTaskName { get; set; }

    }
}