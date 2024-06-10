using GarzoApi.Models;
using GarzoApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace GarzoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
       
        private readonly IAppointmentCollection db;

        public AppointmentController(IAppointmentCollection db)
        {
            this.db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAppointments()
        {
            return Ok(await db.GetAllAppointments());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointmentDetails(int id)
        {

            var appointment = await db.GetAppointmentById(id);
            if (appointment == null)
            {
                return NotFound();
            }
            return Ok(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentModel appointment)
        {
            if (appointment == null)
            {
                return BadRequest();
            }

            if (string.IsNullOrEmpty(appointment.Name) || 
                string.IsNullOrEmpty(appointment.Last_name) || 
                string.IsNullOrEmpty(appointment.Phone_number) || 
                string.IsNullOrEmpty(appointment.Service_type) || 
                appointment.Appointment_date == DateTime.MinValue)
            {
                ModelState.AddModelError("Validation", "All fields except Email are required");
                return BadRequest(ModelState);
            }

            if (string.IsNullOrEmpty(appointment.Email))
            {
                appointment.Email = null;
            }

            await db.CreateDocumentAsync(appointment);

            return CreatedAtAction(nameof(GetAppointmentDetails), new { id = appointment.AppointmentId }, appointment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] AppointmentModel appointment)
        {
            if (appointment == null)
            {
                return BadRequest();
            }

            if (string.IsNullOrEmpty(appointment.Name) || 
                string.IsNullOrEmpty(appointment.Last_name) || 
                string.IsNullOrEmpty(appointment.Phone_number) || 
                string.IsNullOrEmpty(appointment.Service_type) || 
                appointment.Appointment_date == default)
            {
                ModelState.AddModelError("Validation", "All fields except Email are required");
                return BadRequest(ModelState);
            }

           
            appointment.AppointmentId = id; 
            await db.UpdateAppointment(appointment);

            return Created("Created", true);
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {

            await db.DeleteAppointment(id);
           
            return NoContent();
         
        }
    }
}
