using Microsoft.EntityFrameworkCore;
using OnlineBookStore.API.Extensions;
using OnlineBookStore.API.Extenstion;
using OnlineBookStore.Database.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.RegisterDependencies();
builder.Services.RegisterConfiguration(builder.Configuration);
builder.Services.AddAuthentication();
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureSwagger();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
