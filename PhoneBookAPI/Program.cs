using Autofac.Extensions.DependencyInjection;
using Autofac;
using PhoneBookAPI.DataLayer.Contexts;
using Autofac.Core;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace PhoneBookAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterModule(new AutofacBusinessModule());
                });
            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.json");
            builder.Services.AddDbContext<PhoneBookDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("PhonebookSqlDatabase")));

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}