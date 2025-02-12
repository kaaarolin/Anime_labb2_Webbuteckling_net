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

            builder.Services.AddAuthorization();

            // Swagger för API-dokumentation
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
            app.UseAuthorization();

            // POST - Lägg till anime
            app.MapPost("/anime", async ([FromBody] Animes anime) =>
            {
                var animeToAdd = await db.AddAnime("Anime", anime);
                return Results.Ok(animeToAdd);
            }).WithName("AddAnime").WithOpenApi();

            // GET - Hämta alla anime
            app.MapGet("/animes", async () =>
            {
                var anime = await db.GetAllAnime("Anime");
                return Results.Ok(anime);
            }).WithName("GetAllAnimes").WithOpenApi();

            // GET - Hämta anime med ID
            app.MapGet("/anime/{id}", async (string id) =>
            {
                var anime = await db.GetAnimeById("Anime", id); 

                if (anime == null)
                {
                    return Results.NotFound("Error: This anime does not exist");
                }

                return Results.Ok(anime);
            });



            // PUT - Uppdatera anime
            app.MapPut("/anime", async ([FromBody] Animes updateAnime) =>
            {
                var anime = await db.UpdateAnime("Anime", updateAnime);
                return Results.Ok(anime);
            }).WithName("UpdateAnime").WithOpenApi();

            // DELETE - Ta bort anime
            app.MapDelete("/anime/{id}", async (string id) =>
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
