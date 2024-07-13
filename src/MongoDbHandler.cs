using MALSuite.Txt.Models;
using MongoDB.Driver;

namespace MALSuite.Txt;

public class MongoDbHandler
{
    private readonly string connectionString = "mongodb://localhost:27017";
    private readonly string databaseName = "MAL";
    private readonly string animeCollection = "anime";

    public MongoDbHandler()
    {
    }

    private IMongoCollection<T> ConnectToMongo<T>(in string collection)
    {
        var client = new MongoClient(connectionString);
        var db = client.GetDatabase(databaseName);
        return db.GetCollection<T>(collection);
    }

    public async Task<List<DomainAnime>> GetAllAnime()
    {
        var animeCollection = ConnectToMongo<DomainAnime>(this.animeCollection);
        var result = await animeCollection.FindAsync(a => a.Approved == true);
        return result.ToList();
    }
}