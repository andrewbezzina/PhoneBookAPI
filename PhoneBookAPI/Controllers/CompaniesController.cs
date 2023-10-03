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
using PhoneBookAPI.Services;

namespace PhoneBookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly PhoneBookDbContext _context;
        private readonly ICompanyService _companyService;

        public CompaniesController(PhoneBookDbContext context, ICompanyService companyService)
        {
            _context = context;
            _companyService = companyService;
        }

        // GET: api/Companies
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Company>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {
            var companies = await _companyService.GetAll();
            if (companies.IsNullOrEmpty())
            {
                return NotFound();
            }
            return companies.ToList();
        }

        // GET: api/Companies/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Company))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Company>> GetCompany(int id)
        {

            var company = await _companyService.Get(id);

            if (company == null)
            {
                return NotFound();
            }

            return company;
        }

        // POST: api/Companies
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Company))]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Company>> PostCompany(string companyName)
        {
            if (await _companyService.CompanyExists(companyName))
            {
                return Conflict($"Company name '{companyName}' already exists in database.");
            }
            
            var company = await _companyService.Add(companyName);

            return CreatedAtAction("PostCompany", new { id = company.CompanyId }, company);
        }

        // ***************
        // Following endpoints automatically generated, leaving here for utility but would need to be refactored for prod code. 
        // ***************

        // PUT: api/Companies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(int id, Company company)
        {
            if (id != company.CompanyId)
            {
                return BadRequest();
            }

            _context.Entry(company).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Companies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Company>> PostCompany(Company company)
        //{
        //    if (_context.Companies == null)
        //    {
        //        return Problem("Entity set 'PhoneBookDbContext.Companies'  is null.");
        //    }
        //    _context.Companies.Add(company);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetCompany", new { id = company.CompanyId }, company);
        //}

        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            if (_context.Companies == null)
            {
                return NotFound();
            }
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CompanyExists(int id)
        {
            return (_context.Companies?.Any(e => e.CompanyId == id)).GetValueOrDefault();
        }
    }
}
