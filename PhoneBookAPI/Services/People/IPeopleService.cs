using PhoneBookAPI.DataLayer.Models;
using PhoneBookAPI.DataLayer.Models.Request;
using PhoneBookAPI.DataLayer.Models.Response;

namespace PhoneBookAPI.Services.People
{
    public interface IPeopleService
    {
        public Task<Person> Add(AddPerson person);
        public Task<DisplayPerson?> Get(int id);
        public Task<IEnumerable<DisplayPerson>?> GetAll();
        public Task<IEnumerable<DisplayPerson>?> Search(string searchString);
        public Task<Person?> Update(int id);
        public Task<Person?> Remove(int id);
        public Task<Person?> WildCard();
    }
}
