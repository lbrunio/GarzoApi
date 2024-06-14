using GarzoApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics.Metrics;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GarzoApi.Repositories
{
    public class AppointmentCollection : IAppointmentCollection {
        private readonly string COLLECTION = "citas";
        internal MongoDBRepository repository = new MongoDBRepository();
        private readonly IMongoCollection<AppointmentModel> Collection;

        public AppointmentCollection()
        {
            Collection = repository.db.GetCollection<AppointmentModel>(COLLECTION);
       
        } 

        public async Task DeleteAppointment(int id)
        {
            var filter = Builders<AppointmentModel>.Filter.Eq(a => a.AppointmentId, id);
            await Collection.DeleteOneAsync(filter);
        }

        public async Task<List<AppointmentModel>> GetAllAppointments()
        {
            return await Collection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<AppointmentModel> GetAppointmentById(int id)
        {
            var filter = Builders<AppointmentModel>.Filter.Eq(a => a.AppointmentId, id);
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task InsertAppointment(AppointmentModel appointment)
        {
            await Collection.InsertOneAsync(appointment);   
        }

        public async Task UpdateAppointment(AppointmentModel appointment) {
            var filter = Builders<AppointmentModel>.Filter.Eq(a => a.AppointmentId, appointment.AppointmentId);

            
            var update = Builders<AppointmentModel>.Update
                .Set(a => a.Name, appointment.Name) 
                .Set(a => a.Last_name, appointment.Last_name)
                .Set(a => a.Phone_number, appointment.Phone_number)
                .Set(a => a.Email, appointment.Email)
                .Set(a => a.Service_type, appointment.Service_type)
                .Set(a => a.State, appointment.State)
                .Set(a => a.Appointment_date, appointment.Appointment_date);

            var updateResult = await Collection.UpdateOneAsync(filter, update);
        }

        public async Task CreateDocumentAsync(AppointmentModel appointment)
        {
            appointment.AppointmentId = await GetNextSequenceValue(COLLECTION);
            await Collection.InsertOneAsync(appointment);
        }


        // To assign id an int and autoincrement
        private async Task<int> GetNextSequenceValue(string collectionName) {
            var countersCollection = repository.db.GetCollection<CounterModel>("counters");
            var filter = Builders<CounterModel>.Filter.Eq(c => c.Name, collectionName);
            var update = Builders<CounterModel>.Update.Inc(c => c.CurrentValue, 1);
            var options = new FindOneAndUpdateOptions<CounterModel> {
                ReturnDocument = ReturnDocument.After,
                IsUpsert = true
            };

            var counter = await countersCollection.FindOneAndUpdateAsync(filter, update, options);
            return counter.CurrentValue;
        }

    }
}
