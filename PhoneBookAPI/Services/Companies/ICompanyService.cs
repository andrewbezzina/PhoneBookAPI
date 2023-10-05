using PhoneBookAPI.DataLayer.Models;
using PhoneBookAPI.DataLayer.Models.Response;

namespace PhoneBookAPI.Services.Companies
{
    public interface ICompanyService
    {
        public Task<Company> AddAsync(string name);
        public Task<DisplayCompany?> GetAsync(int id);
        public Task<IEnumerable<DisplayCompany>?> GetAllAsync();
        public Task<bool> CompanyExistsAsync(int id);
        public Task<bool> CompanyExistsAsync(string name);
    }
}
