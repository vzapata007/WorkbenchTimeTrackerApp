using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkbenchTimeTrackerApp.Server.Data;
using WorkbenchTimeTrackerApp.Server.DTOs;
using WorkbenchTimeTrackerApp.Server.Models;

namespace WorkbenchTimeTrackerApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeEntriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TimeEntriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/timeentries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TimeEntryDTO>>> GetTimeEntries()
        {
            var timeEntries = await _context.TimeEntries
                .Select(te => new TimeEntryDTO
                {
                    Id = te.Id,
                    EntryDateTime = te.EntryDateTime,
                    PersonId = te.PersonId,
                    WorkTaskId = te.WorkTaskId
                })
                .ToListAsync();

            return Ok(timeEntries);
        }

        // GET: api/timeentries/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TimeEntryDTO>> GetTimeEntry(int id)
        {
            var timeEntry = await _context.TimeEntries
                .Where(te => te.Id == id)
                .Select(te => new TimeEntryDTO
                {
                    Id = te.Id,
                    EntryDateTime = te.EntryDateTime,
                    PersonId = te.PersonId,
                    WorkTaskId = te.WorkTaskId
                })
                .FirstOrDefaultAsync();

            if (timeEntry == null)
            {
                return NotFound(new { Message = $"TimeEntry with ID {id} not found." });
            }

            return Ok(timeEntry);
        }

        // POST: api/timeentries
        [HttpPost]
        public async Task<ActionResult<TimeEntryDTO>> CreateTimeEntry(TimeEntryDTO timeEntryDto)
        {
            var timeEntry = new TimeEntry
            {
                EntryDateTime = timeEntryDto.EntryDateTime,
                PersonId = timeEntryDto.PersonId,
                WorkTaskId = timeEntryDto.WorkTaskId
            };

            _context.TimeEntries.Add(timeEntry);
            await _context.SaveChangesAsync();

            timeEntryDto.Id = timeEntry.Id;
            return CreatedAtAction(nameof(GetTimeEntry), new { id = timeEntry.Id }, timeEntryDto);
        }

        // PUT: api/timeentries/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTimeEntry(int id, TimeEntryDTO timeEntryDto)
        {
            if (id != timeEntryDto.Id)
            {
                return BadRequest();
            }

            var timeEntry = await _context.TimeEntries.FindAsync(id);
            if (timeEntry == null)
            {
                return NotFound(new { Message = $"TimeEntry with ID {id} not found." });
            }

            timeEntry.EntryDateTime = timeEntryDto.EntryDateTime;
            timeEntry.PersonId = timeEntryDto.PersonId;
            timeEntry.WorkTaskId = timeEntryDto.WorkTaskId;

            _context.Entry(timeEntry).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/timeentries/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTimeEntry(int id)
        {
            var timeEntry = await _context.TimeEntries.FindAsync(id);
            if (timeEntry == null)
            {
                return NotFound(new { Message = $"TimeEntry with ID {id} not found." });
            }

            _context.TimeEntries.Remove(timeEntry);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}