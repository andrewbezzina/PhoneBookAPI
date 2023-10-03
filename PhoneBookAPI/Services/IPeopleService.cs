using PhoneBookAPI.DataLayer.Models;
using PhoneBookAPI.DataLayer.Models.Output;

namespace PhoneBookAPI.Services
{
    public interface IPeopleService
    {
        public Task<Person> Add(Person person);
        public Task<Person?> Get(int id);
        public Task<IEnumerable<Person>?> GetAll();
        public Task<IEnumerable<Person>?> Search(string searchString);
        public Task<Person?> Update(int id);
        public Task<Person?> Remove(int id);
        public Task<Person?> WildCard();
    }
}
