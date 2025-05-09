using System;
using Microsoft.EntityFrameworkCore;
using Pagila.API.Data;
using Pagila.API.Models;
namespace Pagila.API.Endpoints;

public static class FilmEndPoints
{
    public static void MapFilmEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/films", async (PagilaDbContext db) => await db.Films.ToListAsync());

        app.MapGet("/films/{id}", async (int id, PagilaDbContext db) =>
            await db.Films.FindAsync(id) is Film film ? Results.Ok(film) : Results.NotFound());

        app.MapPost("/films", async (Film film, PagilaDbContext db) =>
        {
            db.Films.Add(film);
            await db.SaveChangesAsync();
            return Results.Created($"/films/{film.FilmId}", film);
        });

        app.MapPut("/films/{id}", async (int id, Film input, PagilaDbContext db) =>
        {
            var film = await db.Films.FindAsync(id);
            if (film is null) return Results.NotFound();

            film.Title = input.Title;
            film.Description = input.Description;
            film.ReleaseYear = input.ReleaseYear;
            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        app.MapDelete("/films/{id}", async (int id, PagilaDbContext db) =>
        {
            var film = await db.Films.FindAsync(id);
            if (film is null) return Results.NotFound();

            db.Films.Remove(film);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}
