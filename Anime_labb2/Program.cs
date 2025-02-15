using Anime_labb2.Data;
using Anime_labb2.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Anime_labb2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var corsPolicy = "_myAllowSpecificOrigins";

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(corsPolicy,
                    policy =>
                    {
                        policy.WithOrigins("http://127.0.0.1:5500", "http://localhost:3000") // L�gg till din frontend och Node.js API
                              .AllowAnyMethod()
                              .AllowAnyHeader();
                    });
            });

            builder.Services.AddAuthorization();

            // Swagger f�r API-dokumentation
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            MongoCRUD db = new MongoCRUD("AnimeList");

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseCors(corsPolicy);
            app.UseAuthorization();

            // POST - L�gg till anime (�ndrat till `/animes`)
            app.MapPost("/animes", async ([FromBody] Animes anime) =>
            {
                var animeToAdd = await db.AddAnime("Anime", anime);
                return Results.Ok(animeToAdd);
            }).WithName("AddAnime").WithOpenApi();

            // GET - H�mta alla anime (redan r�tt)
            app.MapGet("/animes", async () =>
            {
                var anime = await db.GetAllAnime("Anime");
                return Results.Ok(anime);
            }).WithName("GetAllAnimes").WithOpenApi();

            // GET - H�mta anime med ID (�ndrat till `/animes/{id}`)
            app.MapGet("/animes/{id}", async (string id) =>
            {
                var anime = await db.GetAnimeById("Anime", id);

                if (anime == null)
                {
                    return Results.NotFound("Error: This anime does not exist");
                }

                return Results.Ok(anime);
            }).WithName("GetAnimeById").WithOpenApi();

            // PUT - Uppdatera anime (�ndrat till `/animes`)
            app.MapPut("/animes", async ([FromBody] Animes updateAnime) =>
            {
                var anime = await db.UpdateAnime("Anime", updateAnime);
                return Results.Ok(anime);
            }).WithName("UpdateAnime").WithOpenApi();

            // DELETE - Ta bort anime (�ndrat till `/animes/{id}`)
            app.MapDelete("/animes/{id}", async (string id) =>
            {
                var animeToDelete = await db.DeleteAnime("Anime", id);

                if (animeToDelete == "Anime not found.")
                {
                    return Results.NotFound("Error: Anime not found.");
                }

                return Results.Ok(animeToDelete);
            }).WithName("DeleteAnime").WithOpenApi();

            app.Run();
        }
    }
}
