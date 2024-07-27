using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkbenchTimeTrackerApp.Server.Data;
using WorkbenchTimeTrackerApp.Server.DTOs;
using WorkbenchTimeTrackerApp.Server.Models;

namespace WorkbenchTimeTrackerApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PeopleController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/people
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonDTO>>> GetPeople()
        {
            var people = await _context.People
                .Select(p => new PersonDTO
                {
                    Id = p.Id,
                    FullName = p.FullName
                })
                .ToListAsync();

            return Ok(people);
        }

        // GET: api/people/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonDTO>> GetPerson(int id)
        {
            var person = await _context.People
                .Where(p => p.Id == id)
                .Select(p => new PersonDTO
                {
                    Id = p.Id,
                    FullName = p.FullName,
                    WorkTasks = p.TimeEntries
                        .Select(te => new WorkTaskDTO
                        {
                            Id = te.WorkTask.Id,
                            Name = te.WorkTask.Name,
                            Description = te.WorkTask.Description,
                            TimeEntries = te.WorkTask.TimeEntries
                                .Select(t => new TimeEntryDTO
                                {
                                    Id = t.Id,
                                    EntryDateTime = t.EntryDateTime,
                                    PersonId = t.PersonId,
                                    WorkTaskId = t.WorkTaskId
                                }).ToList()
                        }).ToList()
                })
                .FirstOrDefaultAsync();

            if (person == null)
            {
                return NotFound(new { Message = $"Person with ID {id} not found." });
            }

            return Ok(person);
        }
    }
}