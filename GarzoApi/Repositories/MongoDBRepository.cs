using MongoDB.Driver;

namespace GarzoApi.Repositories
{
    
    public class MongoDBRepository
    {
        private readonly string USER_CONN = "mongodb+srv://brunioragellester:Tofu282288@cluster0.6t5dkpd.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0";
        private readonly string DATABASE = "garzitas";

        public readonly MongoClient client;

        public readonly IMongoDatabase db;

        public MongoDBRepository()
        {
            client = new MongoClient(USER_CONN);

            db = client.GetDatabase(DATABASE);
        }

    }
}
