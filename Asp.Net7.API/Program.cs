
using Asp.Net7.API.Core;
using Asp.Net7.API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApiDbContext>(options =>
{
    options.UseInMemoryDatabase(databaseName: "myToDoDB");
});
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); //Dependency Injection
builder.Services.AddScoped<IDataGenerator, DataGenerator>();

var app = builder.Build();
//var injectedService = app.Services.GetService<IDataGenerator>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//DataGenerator içerisindeki datamýzý program aya kalkarken çalýþmasý için inject service yaptýk.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var myDependency = services.GetRequiredService<IDataGenerator>();
    myDependency.Initialize();
}

//Initialize();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
