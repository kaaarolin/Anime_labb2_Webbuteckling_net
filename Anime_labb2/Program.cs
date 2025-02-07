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
                if (!ObjectId.TryParse(id, out ObjectId objectId))
                {
                    return Results.BadRequest("Invalid ID format");
                }

                var anime = await db.GetAnimeById("Anime", objectId);

                if (anime == null)
                {
                    return Results.NotFound("Error: this anime does not exist");
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
                try
                {
                    var objectId = ObjectId.Parse(id);
                    var animeToDelete = await db.DeleteAnime("Anime", objectId);
                    return Results.Ok(animeToDelete);
                }
                catch (Exception)
                {
                    return Results.BadRequest("Invalid ID format");
                }
            }).WithName("DeleteAnime").WithOpenApi();

            app.Run();
        }
    }
}
