using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PhoneBookAPI.DataLayer.Contexts;
using PhoneBookAPI.DataLayer.Models;
using PhoneBookAPI.DataLayer.Models.Response;


namespace PhoneBookAPI.Services.Companies
{
    public class CompanyService : ICompanyService
    {
        private readonly PhoneBookDbContext _context;

        public CompanyService(PhoneBookDbContext context)
        {
            _context = context;
        }

        public async Task<Company> AddAsync(string name)
        {
            if (_context.Companies.IsNullOrEmpty())
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

        public async Task<IEnumerable<DisplayCompany>?> GetAllAsync()
        {
            if (_context.Companies.IsNullOrEmpty())
            {
                return null;
            }

            return await GetCompaniesWithPeopleCount().ToListAsync();
        }


        public async Task<DisplayCompany?> GetAsync(int id)
        {
            if (_context.Companies.IsNullOrEmpty())
            {
                return null;
            }
            return await GetCompaniesWithPeopleCount().Where(dc => dc.CompanyId == id).FirstOrDefaultAsync();
        }

        public async Task<bool> CompanyExistsAsync(int id)
        {
            if (_context.Companies.IsNullOrEmpty())
            {
                return false;
            }
            return await _context.Companies.AnyAsync(c => c.CompanyId == id);
        }

        public async Task<bool> CompanyExistsAsync(string name)
        {
            if (_context.Companies.IsNullOrEmpty())
            {
                return false;
            }
            return await _context.Companies.AnyAsync(c => c.Name == name);
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
