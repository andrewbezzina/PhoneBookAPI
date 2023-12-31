﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PhoneBookAPI.DataLayer.Models;
using PhoneBookAPI.DataLayer.Models.Request;
using PhoneBookAPI.DataLayer.Models.Response;
using PhoneBookAPI.Services.Companies;
using PhoneBookAPI.Services.People;

namespace PhoneBookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly IPeopleService _peopleService;

        public PeopleController(ICompanyService companyService, IPeopleService peopleService)
        {
            _companyService = companyService;
            _peopleService = peopleService;
        }

        // GET: api/People
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DisplayPerson>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DisplayPerson>>> GetPersons()
        {
            var people = await _peopleService.GetAllAsync();
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
            var person = await _peopleService.GetAsync(id);

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
            var people = await _peopleService.SearchAsync(searchString);
            if (people.IsNullOrEmpty())
            {
                return NotFound();
            }
            return people.ToList();
        }

        // GET: api/People/wildcard
        [HttpGet("wildcard")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DisplayPerson))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DisplayPerson>> WildCard()
        {
            var person = await _peopleService.WildCardAsync();

            if (person == null)
            {
                return NotFound();
            }

            return person;
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

            if (person == null || !await _companyService.CompanyExistsAsync(person.CompanyId))
            {
                return NotFound($"Company with id: {person.CompanyId} not found");
            }

            var ret = await _peopleService.UpdateAsync(id, person);
            if ( ret == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/People
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Person))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<ActionResult<Person>> PostPerson(PersonDetails person)
        {
            if(person == null || !await _companyService.CompanyExistsAsync(person.CompanyId))
            {
                return NotFound($"Company with id: {person.CompanyId} not found");
            }

            var retPerson = await _peopleService.AddAsync(person);
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
            var person = await _peopleService.RemoveAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            return NoContent();
        }


    }
}
