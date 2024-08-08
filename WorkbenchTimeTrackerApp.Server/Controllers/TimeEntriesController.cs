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
        private readonly IUnitOfWork _unitOfWork;

        // Constructor: Injects IUnitOfWork to use for database operations
        public TimeEntriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Retrieves all time entries.
        /// </summary>
        /// <returns>List of TimeEntryDTO.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TimeEntryDTO>>> GetTimeEntries()
        {
            try
            {
                var timeEntries = await _unitOfWork.TimeEntries.GetAllAsync();
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
                return StatusCode(500, new { Message = "An error occurred while retrieving time entries.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves a specific time entry by ID.
        /// </summary>
        /// <param name="id">ID of the time entry.</param>
        /// <returns>TimeEntryDTO if found, otherwise NotFound.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TimeEntryDTO>> GetTimeEntry(int id)
        {
            try
            {
                var timeEntry = await _unitOfWork.TimeEntries.GetByIdAsync(id);
                if (timeEntry == null)
                {
                    return NotFound(new { Message = $"TimeEntry with ID {id} not found." });
                }

                var person = await _unitOfWork.Persons.GetByIdAsync(timeEntry.PersonId);
                var workTask = await _unitOfWork.WorkTasks.GetByIdAsync(timeEntry.WorkTaskId);

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
                return StatusCode(500, new { Message = "An error occurred while retrieving the time entry.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Creates a new time entry.
        /// </summary>
        /// <param name="timeEntryDto">TimeEntryDTO containing time entry details.</param>
        /// <returns>The created TimeEntryDTO.</returns>
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

                await _unitOfWork.TimeEntries.AddAsync(timeEntry);
                await _unitOfWork.CompleteAsync();

                var person = await _unitOfWork.Persons.GetByIdAsync(timeEntry.PersonId);
                var workTask = await _unitOfWork.WorkTasks.GetByIdAsync(timeEntry.WorkTaskId);

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
                return StatusCode(500, new { Message = "An error occurred while creating the time entry.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Updates an existing time entry.
        /// </summary>
        /// <param name="id">ID of the time entry.</param>
        /// <param name="timeEntryDto">TimeEntryDTO containing updated details.</param>
        /// <returns>NoContent if update is successful, otherwise error message.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTimeEntry(int id, TimeEntryDTO timeEntryDto)
        {
            try
            {
                if (id != timeEntryDto.Id)
                {
                    return BadRequest(new { Message = "ID mismatch." });
                }

                var timeEntry = await _unitOfWork.TimeEntries.GetByIdAsync(id);
                if (timeEntry == null)
                {
                    return NotFound(new { Message = $"TimeEntry with ID {id} not found." });
                }

                timeEntry.EntryDateTime = timeEntryDto.EntryDateTime;
                timeEntry.PersonId = timeEntryDto.PersonId;
                timeEntry.WorkTaskId = timeEntryDto.WorkTaskId;

                await _unitOfWork.TimeEntries.UpdateAsync(timeEntry);
                await _unitOfWork.CompleteAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the time entry.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Deletes a specific time entry by ID.
        /// </summary>
        /// <param name="id">ID of the time entry.</param>
        /// <returns>NoContent if deletion is successful, otherwise error message.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTimeEntry(int id)
        {
            try
            {
                var timeEntry = await _unitOfWork.TimeEntries.GetByIdAsync(id);
                if (timeEntry == null)
                {
                    return NotFound(new { Message = $"TimeEntry with ID {id} not found." });
                }

                await _unitOfWork.TimeEntries.DeleteAsync(id);
                await _unitOfWork.CompleteAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the time entry.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves time entries for a specific person by their ID.
        /// </summary>
        /// <param name="personId">ID of the person.</param>
        /// <returns>List of TimeEntryDTO.</returns>
        [HttpGet("person/{personId}")]
        public async Task<ActionResult<IEnumerable<TimeEntryDTO>>> GetTimeEntriesByPersonId(int personId)
        {
            try
            {
                
                var timeEntries = await _unitOfWork.TimeEntries.GetAllAsync(
                    te => te.PersonId == personId,
                    te => te.Person,  // Include related Person entity
                    te => te.WorkTask // Include related WorkTask entity
                );

                var timeEntriesDto = timeEntries.Select(entry => new TimeEntryDTO
                {
                    Id = entry.Id,
                    EntryDateTime = entry.EntryDateTime,
                    PersonId = entry.PersonId,
                    PersonFullName = entry.Person.FullName,
                    WorkTaskId = entry.WorkTaskId,
                    WorkTaskName = entry.WorkTask.Name
                }).ToList();

                if (!timeEntriesDto.Any())
                {
                    return NotFound(new { Message = $"No time entries found for Person ID {personId}." });
                }

                return Ok(timeEntriesDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving time entries by person.", Details = ex.Message });
            }
        }
    }
}