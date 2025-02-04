using Anime_labb2.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

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

        // Add method
        public async Task<List<Animes>> AddAnime(string table, Animes anime)
        {
            var collection = db.GetCollection<Animes>(table);
            await collection.InsertOneAsync(anime);
            return collection.AsQueryable().ToList();
        }

        // Get all anime 

        public async Task<List<Animes>> GetAllAnime(string table)
        {
            var collection = db.GetCollection<Animes>(table);
            var anime = await collection.AsQueryable().ToListAsync();
            return anime;
        }

        // Get anime by id 

        public async Task<Animes> GetAnimeById(string table, string id)
        {
            var collection = db.GetCollection<Animes>(table);
            var anime = await collection.FindAsync(p => p.Id == id);
            return await anime.FirstOrDefaultAsync();
        }

        // Update anime 

        public async Task<Animes> UpdateAnime(string table, Animes anime)
        {
            var collection = db.GetCollection<Animes>(table);
            await collection.ReplaceOneAsync(p => p.Id == anime.Id, anime);
            return anime;
        }

        // Delete anime 

        public async Task<string> DeleteAnime(string table, string id)
        {
            var collection = db.GetCollection<Animes>(table);
            var AnimeToDelete = await collection.DeleteOneAsync(p => p.Id == id);

            return "Successfully deleted";
        }

    }
}
