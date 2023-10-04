using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PhoneBookAPI.DataLayer.Contexts;
using PhoneBookAPI.DataLayer.Models;
using PhoneBookAPI.DataLayer.Models.Response;


namespace PhoneBookAPI.Services.Companies
{
    public class CompanyService : ICompanyService
    {
        private readonly PhoneBookDbContext _context;

        public CompanyService(PhoneBookDbContext context, IMapper mapper)
        {
            _context = context;
        }

        public async Task<Company> Add(string name)
        {
            if (_context.Companies == null)
            {
                return null; ;
            }

            var company = new Company()
            {
                Name = name,
                RegistrationDate = DateTime.Now,
            };
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
            return company;
        }

        public async Task<IEnumerable<DisplayCompany>?> GetAll()
        {
            if (_context.Companies == null)
            {
                return null;
            }

            return await GetCompaniesWithPeopleCount().ToListAsync();
        }


        public async Task<DisplayCompany?> Get(int id)
        {
            if (_context.Companies == null)
            {
                return null;
            }
            return await GetCompaniesWithPeopleCount().Where(dc => dc.CompanyId == id).FirstOrDefaultAsync();
        }

        public async Task<bool> CompanyExists(string name)
        {
            if (_context.Companies == null)
            {
                return false;
            }
            return await _context.Companies.AnyAsync(e => e.Name == name);
        }

        private IQueryable<DisplayCompany> GetCompaniesWithPeopleCount()
        {
            return _context.Companies.GroupJoin(_context.People,
                c => c.CompanyId,
                p => p.CompanyId,
                (c, cGroup) => new DisplayCompany
                {
                    CompanyId = c.CompanyId,
                    Name = c.Name,
                    RegistrationDate = c.RegistrationDate,
                    NumberOfPeople = cGroup.Count()
                });
        }

    }
}
