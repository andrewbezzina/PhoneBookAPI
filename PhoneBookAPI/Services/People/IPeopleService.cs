using PhoneBookAPI.DataLayer.Models;
using PhoneBookAPI.DataLayer.Models.Request;
using PhoneBookAPI.DataLayer.Models.Response;

namespace PhoneBookAPI.Services.People
{
    public interface IPeopleService
    {
        public Task<Person> AddAsync(PersonDetails person);
        public Task<DisplayPerson?> GetAsync(int id);
        public Task<IEnumerable<DisplayPerson>?> GetAllAsync();
        public Task<IEnumerable<DisplayPerson>?> SearchAsync(string searchString);
        public Task<Person?> UpdateAsync(int id, Person person);
        public Task<Person?> RemoveAsync(int id);
        public Task<DisplayPerson?> WildCardAsync();
    }
}
