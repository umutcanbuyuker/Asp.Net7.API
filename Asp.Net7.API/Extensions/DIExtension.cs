using Asp.Net7.API.Core;
using Asp.Net7.API.Core.Repositories;
using Asp.Net7.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Asp.Net7.API.Extensions
{
    public static class DIExtension
    {
        public static void AddServiceDI(this IServiceCollection service)
        {
            service.AddScoped<IUnitOfWork, UnitOfWork>();                           //Dependency Injection için scope ekleyerek kullanım ömrü belirledik.
            service.AddScoped<IDataGenerator, DataGenerator>();

            service.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());         // AutoMapper

            
            service.AddDbContext<ApiDbContext>(options =>                           // In-memory database connection
            {   
                options.UseInMemoryDatabase(databaseName: "myToDoDb");
            });
        }
    }
}
