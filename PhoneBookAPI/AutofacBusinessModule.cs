using Autofac;
using PhoneBookAPI.Services.Companies;
using PhoneBookAPI.Services.People;

namespace PhoneBookAPI
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CompanyService>().As<ICompanyService>();
            builder.RegisterType<PeopleService>().As<IPeopleService>();
        }

    }
}
