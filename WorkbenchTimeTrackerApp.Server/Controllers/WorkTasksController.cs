using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkbenchTimeTrackerApp.Server.Data;
using WorkbenchTimeTrackerApp.Server.DTOs;
using WorkbenchTimeTrackerApp.Server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkbenchTimeTrackerApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkTasksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WorkTasksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/worktasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkTaskDTO>>> GetWorkTasks()
        {
            var workTasks = await _context.WorkTasks
                .Select(wt => new WorkTaskDTO
                {
                    Id = wt.Id,
                    Name = wt.Name,
                    Description = wt.Description,
                    TimeEntries = wt.TimeEntries.Select(te => new TimeEntryDTO
                    {
                        Id = te.Id,
                        EntryDateTime = te.EntryDateTime,
                        PersonId = te.PersonId,
                        WorkTaskId = te.WorkTaskId
                    }).ToList()
                })
                .ToListAsync();

            return Ok(workTasks);
        }

        // GET: api/worktasks/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkTaskDTO>> GetWorkTask(int id)
        {
            var workTask = await _context.WorkTasks
                .Where(wt => wt.Id == id)
                .Select(wt => new WorkTaskDTO
                {
                    Id = wt.Id,
                    Name = wt.Name,
                    Description = wt.Description,
                    TimeEntries = wt.TimeEntries.Select(te => new TimeEntryDTO
                    {
                        Id = te.Id,
                        EntryDateTime = te.EntryDateTime,
                        PersonId = te.PersonId,
                        WorkTaskId = te.WorkTaskId
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (workTask == null)
            {
                return NotFound(new { Message = $"WorkTask with ID {id} not found." });
            }

            return Ok(workTask);
        }
    }
}