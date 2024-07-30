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
    public class WorkTasksController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        // Constructor: Injects IUnitOfWork to use for database operations
        public WorkTasksController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Retrieves all work tasks.
        /// </summary>
        /// <returns>List of WorkTaskDTO.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkTaskDTO>>> GetWorkTasks()
        {
            try
            {
                var workTasks = await _unitOfWork.WorkTasks.GetAllAsync();

                if (!workTasks.Any())
                {
                    return NotFound(new { Message = "No work tasks found." });
                }

                var timeEntries = await _unitOfWork.TimeEntries.GetAllAsync();
                var timeEntriesDictionary = timeEntries
                    .GroupBy(te => te.WorkTaskId)
                    .ToDictionary(group => group.Key, group => group.ToList());

                var workTasksDto = workTasks.Select(wt => new WorkTaskDTO
                {
                    Id = wt.Id,
                    Name = wt.Name,
                    Description = wt.Description,
                    TimeEntries = timeEntriesDictionary.TryGetValue(wt.Id, out var entries)
                        ? entries.Select(te => new TimeEntryDTO
                        {
                            Id = te.Id,
                            EntryDateTime = te.EntryDateTime,
                            PersonId = te.PersonId,
                            WorkTaskId = te.WorkTaskId
                        }).ToList()
                        : new List<TimeEntryDTO>()
                }).ToList();

                return Ok(workTasksDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving work tasks.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves a specific work task by ID.
        /// </summary>
        /// <param name="id">ID of the work task.</param>
        /// <returns>WorkTaskDTO if found, otherwise NotFound.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkTaskDTO>> GetWorkTask(int id)
        {
            try
            {
                var workTask = await _unitOfWork.WorkTasks.GetByIdAsync(id);

                if (workTask == null)
                {
                    return NotFound(new { Message = $"WorkTask with ID {id} not found." });
                }

                var timeEntries = await _unitOfWork.TimeEntries.GetAllAsync();
                var filteredTimeEntries = timeEntries
                    .Where(te => te.WorkTaskId == id)
                    .Select(te => new TimeEntryDTO
                    {
                        Id = te.Id,
                        EntryDateTime = te.EntryDateTime,
                        PersonId = te.PersonId,
                        WorkTaskId = te.WorkTaskId
                    })
                    .ToList();

                var workTaskDto = new WorkTaskDTO
                {
                    Id = workTask.Id,
                    Name = workTask.Name,
                    Description = workTask.Description,
                    TimeEntries = filteredTimeEntries
                };

                return Ok(workTaskDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving the work task.", Details = ex.Message });
            }
        }
    }
}