using Microsoft.AspNetCore.Mvc;
using PhoneBookAPI.DataLayer.Models;
using PhoneBookAPI.DataLayer.Models.Response;

namespace PhoneBookAPI.Services.Companies
{
    public interface ICompanyService
    {
        public Task<Company> Add(string name);
        public Task<DisplayCompany?> Get(int id);
        public Task<IEnumerable<DisplayCompany>?> GetAll();
        public Task<bool> CompanyExists(string name);
    }
}
