using ClinAgenda.src.Application.UseCases;
using ClinAgendaDemo.src.Application.DTOs.Appointment;
using ClinAgendaDemo.src.Application.UseCases;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ClinAgendaDemo.src.WebAPI.Controllers
{
    [ApiController]
    [Route("api/appointment")]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentUseCase _appointmentUseCase;

        public AppointmentController(AppointmentUseCase appointmentUseCase)
        {
            _appointmentUseCase = appointmentUseCase;

        }

        [HttpPost("insert")]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentInsertDTO request)
        {
            try
            {
                var appointmentCreatedId = await _appointmentUseCase.Create(request);
                if (!(appointmentCreatedId > 0)) return StatusCode(500, "Erro ao criar Appointment.");

                return Ok($"Agendamento {appointmentCreatedId} criado com sucesso!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do Servidor: {ex.Message}");
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateAppointment(int id, [FromBody] AppointmentUpdateDTO request)
        {
            try
            {
                var updated = await _appointmentUseCase.Update(id, request);
                if (!updated) return StatusCode(500, "Erro ao atualizar Appointment.");

                return Ok($"Agendamento {id} atualizado com sucesso!");

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do Servidor: {ex.Message}");
            }
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAppointments(
            string? patientName, string? doctorName, int? specialtyId, int itemsPerPage = 10, int page = 1)
        {
            try
            {
                var result = await _appointmentUseCase.GetAll(patientName, doctorName, specialtyId, itemsPerPage, page);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do Servidor: {ex.Message}");
            }
        }

        [HttpGet("listById/{id}")]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            try
            {
                var response = await _appointmentUseCase.GetPatientByIdAsync(id);
                if (response == null) return NotFound($"Appointment com ID {id} não encontrado.");

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do Servidor: {ex.Message}");
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            try
            {
                var appointmentDeleted = await _appointmentUseCase.Delete(id);
                if (!appointmentDeleted) return NotFound($"Erro ao apagar o agendamento {id}.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do Servidor: {ex.Message}");
            }
        }
    }
}