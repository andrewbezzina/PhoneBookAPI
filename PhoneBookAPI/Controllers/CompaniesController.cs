using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PhoneBookAPI.DataLayer.Contexts;
using PhoneBookAPI.DataLayer.Models;
using PhoneBookAPI.DataLayer.Models.Response;
using PhoneBookAPI.Services.Companies;

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
        public async Task<ActionResult<IEnumerable<DisplayCompany>>> GetCompanies()
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
        public async Task<ActionResult<DisplayCompany>> GetCompany(int id)
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

    }
}
