using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkbenchTimeTrackerApp.Server.DTOs
{
    public class PersonDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, ErrorMessage = "Full name can't be longer than 100 characters.")]
        public required string FullName { get; set; }

        // Include WorkTasks with their TimeEntries
        public List<WorkTaskDTO> WorkTasks { get; set; } = new List<WorkTaskDTO>();
    }
}