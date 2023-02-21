
using Asp.Net7.API.Core;
using Asp.Net7.API.Extensions;
using Asp.Net7.API.Validators;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson().AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateToDoValidator>());


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddServiceDI();

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
