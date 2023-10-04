using AutoMapper;
using Microsoft.CodeAnalysis.Differencing;
using PhoneBookAPI.DataLayer.Models;
using PhoneBookAPI.DataLayer.Models.Request;
using PhoneBookAPI.DataLayer.Models.Response;

namespace PhoneBookAPI.Services.People
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<PersonDetails, Person>();
            //CreateMap<Person, DisplayPerson>();
        }
    }
}
