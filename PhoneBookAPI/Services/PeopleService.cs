using PhoneBookAPI.DataLayer.Contexts;
using PhoneBookAPI.DataLayer.Models;

namespace PhoneBookAPI.Services
{
    public class PeopleService : IPeopleService
    {
        private readonly PhoneBookDbContext _context;

        public PeopleService(PhoneBookDbContext context)
        {
            _context = context;
        }

        public async Task<Person> Add(Person person)
        {
            if (_context.People == null)
            {
                return null; ;
            }

            _context.People.Add(person);
            await _context.SaveChangesAsync();
            return person;
        }

        public async Task<Person?> Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Person>?> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<Person?> Remove(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Person>?> Search(string searchString)
        {
            throw new NotImplementedException();
        }

        public async Task<Person?> Update(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Person?> WildCard()
        {
            throw new NotImplementedException();
        }
    }
}
