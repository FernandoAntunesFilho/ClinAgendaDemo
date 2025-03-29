using ClinAgenda.src.Application.UseCases;
using ClinAgendaAPI.StatusUseCase;
using ClinAgendaDemo.src.Application.DTOs.Doctor;
using ClinAgendaDemo.src.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace ClinAgendaDemo.src.WebAPI.Controllers
{
    [ApiController]
    [Route("api/doctor")]
    public class DoctorController : ControllerBase
    {
        private readonly DoctorUseCase _doctorUseCase;
        private readonly SpecialtyUseCase _specialtyUseCase;
        private readonly StatusUseCase _statusUseCase;

        public DoctorController(DoctorUseCase doctorUseCase, SpecialtyUseCase specialtyUseCase, StatusUseCase statusUseCase)
        {
            _doctorUseCase = doctorUseCase;
            _specialtyUseCase = specialtyUseCase;
            _statusUseCase = statusUseCase;
        }

        [HttpGet("listById/{id}")]
        public async Task<IActionResult> GetDoctorByIdAsync(int id)
        {
            try
            {
                var response = await _doctorUseCase.GetDoctorByIdAsync(id);
                if (response == null) return NotFound($"Doutor com ID {id} não encontrado.");

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetDoctorssAsync(
            [FromQuery] string? name,
            [FromQuery] int? specialtyId,
            [FromQuery] int? statusId,
            [FromQuery] int itemsPerPage = 10,
            [FromQuery] int page = 1)
        {
            try
            {
                var result = await _doctorUseCase.GetDoctorsAsync(name, specialtyId, statusId, itemsPerPage, page);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpPost("insert")]
        public async Task<IActionResult> CreateDoctorAsync([FromBody] DoctorInsertDTO doctor)
        {
            try
            {
                foreach (var specialty in doctor.Specialties)
                {
                    var specialtyExists = await _specialtyUseCase.GetSpecialtyByIdAsync(specialty);
                    if (specialtyExists == null) return NotFound($"A especialidade com ID {specialty} não existe");
                }

                var createdDoctor = await _doctorUseCase.CreateDoctorAsync(doctor);

                if (createdDoctor == null)
                    return StatusCode(500, "Erro ao criar Doutor.");

                return Ok(createdDoctor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateDoctorAsync(int id, [FromBody] DoctorUpdateDTO request)
        {
            try
            {
                if (request == null) return BadRequest();
                var hasStatus = await _statusUseCase.GetStatusByIdAsync(request.StatusId);
                if (hasStatus == null)
                    return BadRequest($"O status ID {request.StatusId} não existe");

                bool updated = await _doctorUseCase.UpdateDoctorAsync(id, request);
                if (!updated) return NotFound("Doutor não encontrado.");

                var infosDoctorUpdate = await _doctorUseCase.GetDoctorByIdAsync(id);
                return Ok(infosDoctorUpdate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }
    }
}