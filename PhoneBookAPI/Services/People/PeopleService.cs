using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PhoneBookAPI.DataLayer.Contexts;
using PhoneBookAPI.DataLayer.Models;
using PhoneBookAPI.DataLayer.Models.Request;
using PhoneBookAPI.DataLayer.Models.Response;

namespace PhoneBookAPI.Services.People
{
    public class PeopleService : IPeopleService
    {
        private readonly PhoneBookDbContext _context;
        private readonly IMapper _mapper;

        public PeopleService(PhoneBookDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper =mapper;
        }

        public async Task<Person> Add(PersonDetails addPerson)
        {
            if (_context.People == null)
            {
                return null; ;
            }

            var person = _mapper.Map<Person>(addPerson);
            _context.People.Add(person);
            await _context.SaveChangesAsync();
            return person;
        }

        public async Task<DisplayPerson?> Get(int id)
        {
            if (_context.People == null || _context.Companies == null)
            {
                return null;
            }

            return await GetPeopleWithCompanyName().Where(dp => dp.PersonId == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<DisplayPerson>?> GetAll()
        {
            if (_context.People == null)
            {
                return null;
            }

            return await GetPeopleWithCompanyName().ToListAsync();
        }

        public async Task<Person?> Remove(int id)
        {
            if (_context.People == null)
            {
                return null;
            }
            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return null;
            }

            _context.People.Remove(person);
            await _context.SaveChangesAsync();
            return person;
        }

        public async Task<IEnumerable<DisplayPerson>?> Search(string searchString)
        {
            if (_context.People == null)
            {
                return null;
            }
            return await GetPeopleWithCompanyName().Where(p => p.FullName.Contains(searchString)
                                                    || p.Address.Contains(searchString) 
                                                    || p.PhoneNumber.Contains(searchString)
                                                    || p.CompanyName.Contains(searchString))
                                                    .ToListAsync();
            
        }

        public async Task<Person?> Update(int id, Person person)
        {
            if (_context.People == null)
            {
                return null;
            }

            _context.Entry(person).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return person;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<Person?> WildCard()
        {
            if (_context.People == null)
            {
                return null;
            }
            throw new NotImplementedException();
        }

        private IQueryable<DisplayPerson> GetPeopleWithCompanyName()
        {
            return _context.People.Join(_context.Companies,
                                           p => p.CompanyId,
                                           c => c.CompanyId,
                                           (p, c) => new DisplayPerson
                                           {
                                               PersonId = p.PersonId,
                                               FullName = p.FullName,
                                               PhoneNumber = p.PhoneNumber,
                                               Address = p.Address,
                                               CompanyId = p.CompanyId,
                                               CompanyName = c.Name
                                           });
        }

        private bool PersonExists(int id)
        {
            return (_context.People?.Any(e => e.PersonId == id)).GetValueOrDefault();
        }
    }
}
