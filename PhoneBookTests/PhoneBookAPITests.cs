using Autofac.Core;
using AutoMapper;
using Humanizer;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;
using PhoneBookAPI.DataLayer.Contexts;
using PhoneBookAPI.DataLayer.Models;
using PhoneBookAPI.DataLayer.Models.Request;
using PhoneBookAPI.Services.Companies;
using PhoneBookAPI.Services.People;
using System.Data.Common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace PhoneBookTests
{
    [TestClass]
    public class PhoneBookAPITests : IDisposable
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<PhoneBookDbContext> _contextOptions;
        private readonly IMapper _mapper;

        // Sample Db data:
        private readonly Company company1_Mircosoft = new Company { CompanyId = 1, Name = "Microsoft", RegistrationDate = DateTime.Parse("10/04/2023") };
        private readonly Company company2_Facebook = new Company { CompanyId = 2, Name = "Facebook", RegistrationDate = DateTime.Parse("10/03/2023") };
        private readonly Person person1 = new Person { PersonId = 1, FullName = "Jon Smith", Address = "1, High street", PhoneNumber = "21123456", CompanyId = 1 };
        private readonly Person person2 = new Person { PersonId = 2, FullName = "Jon White", Address = "50, Free Street", PhoneNumber = "21234567", CompanyId = 1 };

        #region ConstructorAndDispose
        public PhoneBookAPITests()
        {
            // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
            // at the end of the test (see Dispose below).
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            // These options will be used by the context instances in this test suite, including the connection opened above.
            _contextOptions = new DbContextOptionsBuilder<PhoneBookDbContext>()
                .UseSqlite(_connection)
                .Options;

            // Create the schema and seed some data
            using var context = new PhoneBookDbContext(_contextOptions);

            if (context.Database.EnsureCreated())
            {
                using var viewCommand = context.Database.GetDbConnection().CreateCommand();
                viewCommand.CommandText = @"
CREATE VIEW AllResources AS
SELECT Name
FROM Companies;";
                viewCommand.ExecuteNonQuery();
            }

            context.AddRange(company1_Mircosoft, company2_Facebook, person1, person2);
            context.SaveChanges();

            // Add Mapper
            var mappingprofile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingprofile));
            _mapper = new Mapper(configuration);

        }

        PhoneBookDbContext CreateContext() => new PhoneBookDbContext(_contextOptions);

        public void Dispose() => _connection.Dispose();
        #endregion

        [TestMethod]
        public async Task Company_Add()
        {
            using var context = CreateContext();
            const string addCompanyName = "Google";
            var companyService = new CompanyService(context);
            var company = await companyService.AddAsync(addCompanyName);
            Assert.IsNotNull(company);
            Assert.AreEqual(company.Name, addCompanyName);
            Assert.AreEqual(context.Companies.Count(), 3);
            Assert.IsTrue(context.Companies.Any(c => c.Name == addCompanyName));
            Assert.AreEqual(context.Companies.FirstOrDefault(c => c.Name == addCompanyName).RegistrationDate.Date, DateTime.Today);
        }

        [TestMethod]
        public async Task Company_GetAll()
        {
            using var context = CreateContext();
            var companyService = new CompanyService(context);

            var companies = await companyService.GetAllAsync();

            Assert.IsNotNull(companies);
            Assert.AreEqual(companies.Count(), 2);
            Assert.AreEqual(companies.FirstOrDefault(c => c.CompanyId == 1).Name, company1_Mircosoft.Name);
            Assert.AreEqual(companies.FirstOrDefault(c => c.CompanyId == 1).RegistrationDate, company1_Mircosoft.RegistrationDate);
            Assert.AreEqual(companies.FirstOrDefault(c => c.CompanyId == 1).NumberOfPeople, 2);
            Assert.AreEqual(companies.FirstOrDefault(c => c.CompanyId == 2).Name, company2_Facebook.Name);
            Assert.AreEqual(companies.FirstOrDefault(c => c.CompanyId == 2).RegistrationDate, company2_Facebook.RegistrationDate);
            Assert.AreEqual(companies.FirstOrDefault(c => c.CompanyId == 2).NumberOfPeople, 0);
        }

        [TestMethod]
        public async Task Person_Add()
        {
            using var context = CreateContext();
            var peopleService = new PeopleService(context, _mapper);
            var personToAdd = new PersonDetails { FullName = "Jane Doe", Address = "23, Black street", PhoneNumber = "21345678", CompanyId = 2 };
            var personAdded = await peopleService.AddAsync(personToAdd);

            Assert.IsNotNull(personAdded);
            Assert.AreEqual(personAdded.FullName, personToAdd.FullName);
            Assert.AreEqual(context.People.Count(), 3);
            Assert.IsTrue(context.People.Any(p => p.FullName == personToAdd.FullName));
            Assert.AreEqual(context.People.Where(p => p.FullName == personToAdd.FullName).FirstOrDefault().Address, personToAdd.Address);
            Assert.AreEqual(context.People.Where(p => p.FullName == personToAdd.FullName).FirstOrDefault().PhoneNumber, personToAdd.PhoneNumber);
            Assert.AreEqual(context.People.Where(p => p.FullName == personToAdd.FullName).FirstOrDefault().CompanyId, personToAdd.CompanyId);
        }

        [TestMethod]
        public async Task Person_Add_Edit_Remove()
        {
            using var context = CreateContext();
            var peopleService = new PeopleService(context, _mapper);
            
            //Add
            var personToAdd = new PersonDetails { FullName = "Jane Doe", Address = "23, Black street", PhoneNumber = "21345678", CompanyId = 2 };
            var personAdded = await peopleService.AddAsync(personToAdd);

            Assert.IsNotNull(personAdded);
            Assert.AreEqual(personAdded.FullName, personToAdd.FullName);
            Assert.AreEqual(context.People.Count(), 3);
            Assert.IsTrue(context.People.Any(p => p.FullName == personToAdd.FullName));
            Assert.AreEqual(context.People.Where(p => p.FullName == personToAdd.FullName).FirstOrDefault().Address, personToAdd.Address);
            Assert.AreEqual(context.People.Where(p => p.FullName == personToAdd.FullName).FirstOrDefault().PhoneNumber, personToAdd.PhoneNumber);
            Assert.AreEqual(context.People.Where(p => p.FullName == personToAdd.FullName).FirstOrDefault().CompanyId, personToAdd.CompanyId);

            //Edit
            string newAddress = "70, New Street";
            personAdded.Address = newAddress;
            personAdded.CompanyId = 2;
            var editedPerson = await peopleService.UpdateAsync(personAdded.PersonId, personAdded);


            Assert.IsNotNull(editedPerson);
            Assert.AreEqual(personAdded, editedPerson);
            Assert.AreEqual(context.People.Count(), 3);
            Assert.AreEqual(context.People.Where(p => p.PersonId == personAdded.PersonId).FirstOrDefault().Address, newAddress);
            Assert.AreEqual(context.People.Where(p => p.PersonId == personAdded.PersonId).FirstOrDefault().CompanyId, personAdded.CompanyId);

            //Remove
            var deletedPerson = await peopleService.RemoveAsync(editedPerson.PersonId);

            Assert.IsNotNull(deletedPerson);
            Assert.AreEqual(context.People.Count(), 2);
            Assert.IsFalse(context.People.Any(p => p.PersonId == deletedPerson.PersonId));
        }

        [TestMethod]
        public async Task Person_GetAll()
        {
            using var context = CreateContext();
            var peopleService = new PeopleService(context, _mapper);

            var people = await peopleService.GetAllAsync();

            Assert.IsNotNull(people);
            Assert.AreEqual(people.Count(), 2);
            Assert.AreEqual(people.FirstOrDefault(c => c.PersonId == 1).FullName, person1.FullName);
            Assert.AreEqual(people.FirstOrDefault(c => c.PersonId == 1).Address, person1.Address);
            Assert.AreEqual(people.FirstOrDefault(c => c.PersonId == 1).PhoneNumber, person1.PhoneNumber);
            Assert.AreEqual(people.FirstOrDefault(c => c.PersonId == 1).CompanyId, person1.CompanyId);
            Assert.AreEqual(people.FirstOrDefault(c => c.PersonId == 1).CompanyName, company1_Mircosoft.Name);
            Assert.AreEqual(people.FirstOrDefault(c => c.PersonId == 2).FullName, person2.FullName);
            Assert.AreEqual(people.FirstOrDefault(c => c.PersonId == 2).Address, person2.Address);
            Assert.AreEqual(people.FirstOrDefault(c => c.PersonId == 2).PhoneNumber, person2.PhoneNumber);
            Assert.AreEqual(people.FirstOrDefault(c => c.PersonId == 2).CompanyId, person2.CompanyId);
            Assert.AreEqual(people.FirstOrDefault(c => c.PersonId == 2).CompanyName, company1_Mircosoft.Name);
        }

        [TestMethod]
        [DataRow("Jon", 2)]
        [DataRow("Smith", 1)]
        [DataRow("Microsoft", 2)]
        [DataRow("Facbook", 0)]
        public async Task Person_Search(string searchTerm, int count)
        {
            using var context = CreateContext();
            var peopleService = new PeopleService(context, _mapper);

            var people = await peopleService.SearchAsync(searchTerm);

            Assert.AreEqual(people.Count(), count);
        }

        [TestMethod]
        public async Task Person_WildCard()
        {
            using var context = CreateContext();
            var peopleService = new PeopleService(context, _mapper);

            var person = await peopleService.WildCardAsync();

            Assert.IsNotNull(person);
            Assert.AreEqual(person.CompanyName, company1_Mircosoft.Name);
        }
    }
}