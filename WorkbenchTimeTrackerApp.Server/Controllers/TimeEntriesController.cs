using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkbenchTimeTrackerApp.Server.DTOs;
using WorkbenchTimeTrackerApp.Server.Models;
using WorkbenchTimeTrackerApp.Server.Repositories;

namespace WorkbenchTimeTrackerApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeEntriesController : ControllerBase
    {
        private readonly IRepository<TimeEntry> _timeEntryRepository;
        private readonly IRepository<Person> _personRepository;
        private readonly IRepository<WorkTask> _workTaskRepository;

        public TimeEntriesController(IRepository<TimeEntry> timeEntryRepository,
            IRepository<Person> personRepository,
            IRepository<WorkTask> workTaskRepository)
        {
            _timeEntryRepository = timeEntryRepository;
            _personRepository = personRepository;
            _workTaskRepository = workTaskRepository;
        }

        // GET: api/timeentries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TimeEntryDTO>>> GetTimeEntries()
        {
            try
            {
                var timeEntries = await _timeEntryRepository.GetAllAsync();
                var timeEntriesDto = timeEntries.Select(te => new TimeEntryDTO
                {
                    Id = te.Id,
                    EntryDateTime = te.EntryDateTime,
                    PersonId = te.PersonId,
                    WorkTaskId = te.WorkTaskId
                }).ToList();

                return Ok(timeEntriesDto);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return StatusCode(500, new { Message = "An error occurred while retrieving time entries.", Details = ex.Message });
            }
        }

        // GET: api/timeentries/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TimeEntryDTO>> GetTimeEntry(int id)
        {
            try
            {
                var timeEntry = await _timeEntryRepository.GetByIdAsync(id);
                if (timeEntry == null)
                {
                    return NotFound(new { Message = $"TimeEntry with ID {id} not found." });
                }

                var person = await _personRepository.GetByIdAsync(timeEntry.PersonId);
                var workTask = await _workTaskRepository.GetByIdAsync(timeEntry.WorkTaskId);

                var timeEntryDto = new TimeEntryDTO
                {
                    Id = timeEntry.Id,
                    EntryDateTime = timeEntry.EntryDateTime,
                    PersonId = timeEntry.PersonId,
                    PersonFullName = person?.FullName,
                    WorkTaskId = timeEntry.WorkTaskId,
                    WorkTaskName = workTask?.Name
                };

                return Ok(timeEntryDto);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return StatusCode(500, new { Message = "An error occurred while retrieving the time entry.", Details = ex.Message });
            }
        }

        // POST: api/timeentries
        [HttpPost]
        public async Task<ActionResult<TimeEntryDTO>> CreateTimeEntry(TimeEntryDTO timeEntryDto)
        {
            try
            {
                var timeEntry = new TimeEntry
                {
                    EntryDateTime = timeEntryDto.EntryDateTime,
                    PersonId = timeEntryDto.PersonId,
                    WorkTaskId = timeEntryDto.WorkTaskId
                };

                await _timeEntryRepository.AddAsync(timeEntry);

                var person = await _personRepository.GetByIdAsync(timeEntry.PersonId);
                var workTask = await _workTaskRepository.GetByIdAsync(timeEntry.WorkTaskId);

                var result = new TimeEntryDTO
                {
                    Id = timeEntry.Id,
                    EntryDateTime = timeEntry.EntryDateTime,
                    PersonId = timeEntry.PersonId,
                    PersonFullName = person?.FullName,
                    WorkTaskId = timeEntry.WorkTaskId,
                    WorkTaskName = workTask?.Name
                };

                return CreatedAtAction(nameof(GetTimeEntry), new { id = timeEntry.Id }, result);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return StatusCode(500, new { Message = "An error occurred while creating the time entry.", Details = ex.Message });
            }
        }

        // PUT: api/timeentries/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTimeEntry(int id, TimeEntryDTO timeEntryDto)
        {
            try
            {
                if (id != timeEntryDto.Id)
                {
                    return BadRequest(new { Message = "ID mismatch." });
                }

                var timeEntry = await _timeEntryRepository.GetByIdAsync(id);
                if (timeEntry == null)
                {
                    return NotFound(new { Message = $"TimeEntry with ID {id} not found." });
                }

                timeEntry.EntryDateTime = timeEntryDto.EntryDateTime;
                timeEntry.PersonId = timeEntryDto.PersonId;
                timeEntry.WorkTaskId = timeEntryDto.WorkTaskId;

                await _timeEntryRepository.UpdateAsync(timeEntry);

                return NoContent();
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return StatusCode(500, new { Message = "An error occurred while updating the time entry.", Details = ex.Message });
            }
        }

        // DELETE: api/timeentries/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTimeEntry(int id)
        {
            try
            {
                var timeEntry = await _timeEntryRepository.GetByIdAsync(id);
                if (timeEntry == null)
                {
                    return NotFound(new { Message = $"TimeEntry with ID {id} not found." });
                }

                await _timeEntryRepository.DeleteAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return StatusCode(500, new { Message = "An error occurred while deleting the time entry.", Details = ex.Message });
            }
        }

        // GET: api/timeentries/person/{personId}
        [HttpGet("person/{personId}")]
        public async Task<ActionResult<IEnumerable<TimeEntryDTO>>> GetTimeEntriesByPersonId(int personId)
        {
            try
            {
                // Fetch all time entries
                var timeEntries = await _timeEntryRepository.GetAllAsync();

                // Filter entries by person ID
                var filteredEntries = timeEntries
                    .Where(te => te.PersonId == personId)
                    .ToList();

                // Return NotFound if no entries are found
                if (filteredEntries.Count == 0) 
                {
                    return NotFound(new { Message = $"No time entries found for Person ID {personId}." });
                }

                // Fetch related people and work tasks
                var people = await _personRepository.GetAllAsync();
                var workTasks = await _workTaskRepository.GetAllAsync();

                // Map filtered entries to DTOs
                var timeEntriesDto = filteredEntries.Select(entry => new TimeEntryDTO
                {
                    Id = entry.Id,
                    EntryDateTime = entry.EntryDateTime,
                    PersonId = entry.PersonId,
                    PersonFullName = people.FirstOrDefault(p => p.Id == entry.PersonId)?.FullName,
                    WorkTaskId = entry.WorkTaskId,
                    WorkTaskName = workTasks.FirstOrDefault(wt => wt.Id == entry.WorkTaskId)?.Name
                }).ToList();

                return Ok(timeEntriesDto);
            }
            catch (Exception ex)
            {
                // Handle errors
                return StatusCode(500, new { Message = "An error occurred while retrieving time entries by person.", Details = ex.Message });
            }
        }
    }
}