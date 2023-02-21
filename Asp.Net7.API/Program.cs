
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

// Geli�tirdi�imiz fake servis ile Dependency injection yapt�k.
// DataGenerator i�erisindeki datam�z� program aya�a kalkarken �al��mas� i�in inject service yapt�k.
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
