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
        private readonly PatientUseCase _patientUseCase;
        private readonly DoctorUseCase _doctorUseCase;
        private readonly SpecialtyUseCase _specialtyUseCase;

        public AppointmentController(
            AppointmentUseCase appointmentUseCase,
            PatientUseCase patientUseCase,
            DoctorUseCase doctorUseCase,
            SpecialtyUseCase specialtyUseCase)
        {
            _appointmentUseCase = appointmentUseCase;
            _patientUseCase = patientUseCase;
            _doctorUseCase = doctorUseCase;
            _specialtyUseCase = specialtyUseCase;
        }

        [HttpPost("insert")]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentInsertDTO request)
        {
            try
            {
                var hasPatient = await _patientUseCase.GetPatientByIdAsync(request.PatientId);
                if (hasPatient == null) return BadRequest($"O Paciente ID {request.PatientId} não existe");

                var hasDoctor = await _doctorUseCase.GetDoctorByIdAsync(request.DoctorId);
                if (hasDoctor == null) return BadRequest($"O Doutor ID {request.DoctorId} não existe");

                var hasSpecialty = await _specialtyUseCase.GetSpecialtyByIdAsync(request.SpecialtyId);
                if (hasSpecialty == null) return BadRequest($"A especalidade ID {request.SpecialtyId} não existe");

                var appointmentCreatedId = await _appointmentUseCase.Create(request);
                if (!(appointmentCreatedId > 0)) return StatusCode(500, "Erro ao criar a Appointment.");

                return Ok($"Agendamento {appointmentCreatedId} criado com sucesso!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do Servidor: {ex.Message}");
            }
        }
    }
}