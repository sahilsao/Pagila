using Microsoft.EntityFrameworkCore;
using Pagila.API.Configurations;
using Pagila.API.Data;
using Pagila.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PagilaDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

 builder.Services.AddOpenApiConfiguration();   
//builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseOpenApiConfiguration();

app.UseHttpsRedirection();

// Register endpoints
app.MapFilmEndpoints();

app.Run();

