using PhoneBookAPI.DataLayer.Models;
using PhoneBookAPI.DataLayer.Models.Request;
using PhoneBookAPI.DataLayer.Models.Response;

namespace PhoneBookAPI.Services.People
{
    public interface IPeopleService
    {
        public Task<Person> Add(PersonDetails person);
        public Task<DisplayPerson?> Get(int id);
        public Task<IEnumerable<DisplayPerson>?> GetAll();
        public Task<IEnumerable<DisplayPerson>?> Search(string searchString);
        public Task<Person?> Update(int id, Person person);
        public Task<Person?> Remove(int id);
        public Task<Person?> WildCard();
    }
}
