
using Anime_labb2.Data;
using Anime_labb2.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Anime_labb2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            MongoCRUD db = new MongoCRUD("AnimeList");

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            // Post metod 

            app.MapPost("/anime", async (Animes anime) =>
            {
                var animeToAdd = await db.AddAnime("Anime", anime);
                return Results.Ok(animeToAdd);
            });

            // Get all metod 

            app.MapGet("/animes", async () =>
            {
                var anime = await db.GetAllAnime("Anime");
                return Results.Ok(anime);
            });

            // Get by id method 

            app.MapGet("/anime/{id}", async (string id) =>
            {
                var anime = await db.GetAnimeById("Anime", id);

                if (anime == null)
                {
                    return Results.NotFound("Error: this anime does not exist");
                }

                return Results.Ok(anime);
            });

            // Update method 

            app.MapPut("/anime", async (Animes UpdateAnime) =>
            {
                var anime = await db.UpdateAnime("Anime", UpdateAnime);
                return Results.Ok(anime);
            });

            // Delete method 

            app.MapDelete("/anime/{id}", async (string id) =>
            {
                var animeToDelete = await db.DeleteAnime("Anime", id);
                return Results.Ok(animeToDelete);
            });

            app.Run();
        }
    }
}
