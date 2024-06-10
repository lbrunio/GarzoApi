using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GarzoApi.Models {
    public class CounterModel {

        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; } // MongoDb - Collection name
        public int CurrentValue { get; set; }

    }
}
