using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkbenchTimeTrackerApp.Server.DTOs
{
    public class WorkTaskDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string? Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string? Description { get; set; }

        // Include TimeEntries related to this WorkTask
        public List<TimeEntryDTO> TimeEntries { get; set; } = new List<TimeEntryDTO>();
    }
}