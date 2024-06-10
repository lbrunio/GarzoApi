using GarzoApi.Models;

namespace GarzoApi.Repositories
{
    public interface IAppointmentCollection
    {
        Task InsertAppointment(AppointmentModel appointment);

        Task UpdateAppointment(AppointmentModel appointment);

        Task DeleteAppointment(int id);

        Task<List<AppointmentModel>> GetAllAppointments();

        Task<AppointmentModel> GetAppointmentById(int id);
        Task CreateDocumentAsync(AppointmentModel appointment);
    }
}
