using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneBookAPI.DataLayer.Contexts;
using PhoneBookAPI.DataLayer.Models;
using PhoneBookAPI.DataLayer.Models.Output;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PhoneBookAPI.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly PhoneBookDbContext _context;

        public CompanyService(PhoneBookDbContext context)
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


        //TODO: add employee count functionality
        public async Task<IEnumerable<DisplayCompany>?> GetAll()
        {
            if (_context.Companies == null)
            {
                return null;
            }

            return await _context.Companies.Select(c => new DisplayCompany
            {
                CompanyId = c.CompanyId,
                Name = c.Name,
                RegistrationDate = c.RegistrationDate
            }).ToListAsync();
            
        }

        
        public async Task<DisplayCompany?> Get(int id)
        {
            if (_context.Companies == null)
            {
                return null;
            }
            var company = await _context.Companies.FindAsync(id);
            var DisplayCompany = new DisplayCompany
            {
                CompanyId = company.CompanyId,
                Name = company.Name,
                RegistrationDate = company.RegistrationDate
            };
            //TODO: add employee count functionality

            return DisplayCompany;
        }

        public async Task<bool> CompanyExists(string name)
        {
            if (_context.Companies == null)
            {
                return false;
            }
            return await _context.Companies.AnyAsync(e => e.Name == name);
        }

        private bool CompanyExists(int id)
        {
            return (_context.Companies?.Any(e => e.CompanyId == id)).GetValueOrDefault();
        }


    }
}
