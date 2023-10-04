using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PhoneBookAPI.DataLayer.Contexts;
using PhoneBookAPI.DataLayer.Models;
using PhoneBookAPI.DataLayer.Models.Request;
using PhoneBookAPI.DataLayer.Models.Response;
using PhoneBookAPI.Services.People;

namespace PhoneBookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly PhoneBookDbContext _context;
        private readonly IPeopleService _peopleService;

        public PeopleController(PhoneBookDbContext context, IPeopleService peopleService)
        {
            _context = context;
            _peopleService = peopleService;
        }

        // GET: api/People
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DisplayPerson>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DisplayPerson>>> GetPersons()
        {
            var people = await _peopleService.GetAll();
            if (people.IsNullOrEmpty())
            {
                return NotFound();
            }
            return people.ToList();
        }

        // GET: api/People/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DisplayPerson))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DisplayPerson>> GetPerson(int id)
        {
            var person = await _peopleService.Get(id);

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }

        // GET: api/People/John
        [HttpGet("search/{searchString}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DisplayPerson>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<DisplayPerson>>> SearchPerson(string searchString)
        {
            var people = await _peopleService.Search(searchString);
            if (people.IsNullOrEmpty())
            {
                return NotFound();
            }
            return people.ToList();
        }

        // PUT: api/People/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutPerson(int id, Person person)
        {
            if (id != person.PersonId)
            {
                return BadRequest();
            }

            var ret = await _peopleService.Update(id, person);
            if ( ret == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/People
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Person))]
        public async Task<ActionResult<Person>> PostPerson(PersonDetails person)
        {

            var retPerson = await _peopleService.Add(person);
            if (retPerson == null)
            {
                return Problem("Unable to connect to DB");
            }

            return CreatedAtAction("PostPerson", new { id = retPerson.PersonId }, retPerson);
        }

        // DELETE: api/People/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var person = await _peopleService.Remove(id);
            if (person == null)
            {
                return NotFound();
            }

            return NoContent();
        }


    }
}
