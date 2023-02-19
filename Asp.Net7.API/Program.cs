
using Asp.Net7.API.Core;
using Asp.Net7.API.Data;
using Asp.Net7.API.Middlewares;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // AutoMapper
builder.Services.AddDbContext<ApiDbContext>(options =>
{
    options.UseInMemoryDatabase(databaseName: "myToDoDB");
});
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); //Dependency Injection için scope ekleyerek kullaným ömrü belirledik.
builder.Services.AddScoped<IDataGenerator, DataGenerator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Geliþtirdiðimiz fake servis ile Dependency injection yaptýk.
// DataGenerator içerisindeki datamýzý program ayaða kalkarken çalýþmasý için inject service yaptýk.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var myDependency = services.GetRequiredService<IDataGenerator>();
    myDependency.Initialize();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.UseExceptionHandleMiddle();

app.MapControllers();

app.Run();
