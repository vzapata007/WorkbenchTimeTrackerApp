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
    public class PeopleController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        // Constructor: Injects IUnitOfWork to use for database operations
        public PeopleController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Retrieves all people.
        /// </summary>
        /// <returns>List of PersonDTO.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonDTO>>> GetPeople()
        {
            try
            {
                var people = await _unitOfWork.Persons.GetAllAsync();

                if (!people.Any())
                {
                    return NotFound(new { Message = "No people found." });
                }

                var peopleDto = people.Select(p => new PersonDTO
                {
                    Id = p.Id,
                    FullName = p.FullName
                }).ToList();

                return Ok(peopleDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving people.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves a specific person by ID.
        /// </summary>
        /// <param name="id">ID of the person.</param>
        /// <returns>PersonDTO if found, otherwise NotFound.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonDTO>> GetPerson(int id)
        {
            try
            {
                var person = await _unitOfWork.Persons.GetByIdAsync(id);

                if (person == null)
                {
                    return NotFound(new { Message = $"Person with ID {id} not found." });
                }

                var personDto = new PersonDTO
                {
                    Id = person.Id,
                    FullName = person.FullName
                };

                return Ok(personDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving the person.", Details = ex.Message });
            }
        }
    }
}