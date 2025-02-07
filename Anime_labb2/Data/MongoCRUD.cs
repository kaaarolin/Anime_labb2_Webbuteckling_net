using Anime_labb2.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Bson;

namespace Anime_labb2.Data
{
    public class MongoCRUD
    {
        private IMongoDatabase db;

        public MongoCRUD(string database)
        {
            var client = new MongoClient();
            db = client.GetDatabase(database);
        }

        // 🔹 ADD: Lägg till anime
        public async Task<List<Animes>> AddAnime(string table, Animes anime)
        {
            var collection = db.GetCollection<Animes>(table);
            await collection.InsertOneAsync(anime);
            return collection.AsQueryable().ToList();
        }

        // 🔹 GET ALL: Hämta alla anime
        public async Task<List<Animes>> GetAllAnime(string table)
        {
            var collection = db.GetCollection<Animes>(table);
            return await collection.AsQueryable().ToListAsync();
        }

        // 🔹 FIXAD GET BY ID: Hämta anime via ID (använder ObjectId)
        public async Task<Animes> GetAnimeById(string table, ObjectId id)
        {
            var collection = db.GetCollection<Animes>(table);
            return await collection.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        // 🔹 FIXAD UPDATE: Uppdatera anime (använder ObjectId)
        public async Task<Animes> UpdateAnime(string table, Animes anime)
        {
            var collection = db.GetCollection<Animes>(table);
            await collection.ReplaceOneAsync(p => p.Id == anime.Id, anime);
            return anime;
        }

        // 🔹 FIXAD DELETE: Ta bort anime via ID (använder ObjectId)
        public async Task<string> DeleteAnime(string table, ObjectId id)
        {
            var collection = db.GetCollection<Animes>(table);
            var result = await collection.DeleteOneAsync(p => p.Id == id);

            if (result.DeletedCount > 0)
            {
                return $"Anime with ID {id} was successfully deleted";
            }
            else
            {
                return "Anime not found";
            }
        }
    }
}

