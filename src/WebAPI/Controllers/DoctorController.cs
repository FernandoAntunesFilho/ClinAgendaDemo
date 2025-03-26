using ClinAgenda.src.Application.UseCases;
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

        public DoctorController(DoctorUseCase doctorUseCase, SpecialtyUseCase specialtyUseCase)
        {
            _doctorUseCase = doctorUseCase;
            _specialtyUseCase = specialtyUseCase;
        }

        [HttpGet("list/{id}")]
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
            //TODO: Implementar try catch
            foreach (var specialty in doctor.Specialties)
            {
                var specialtyExists = await _specialtyUseCase.GetSpecialtyByIdAsync(specialty);
                if (specialtyExists == null) return NotFound($"A especialidade com ID {specialty} não existe");
            }

            var createdDoctorId = await _doctorUseCase.CreateDoctorAsync(doctor);

            if (!(createdDoctorId > 0))
                return StatusCode(500, "Erro ao criar Doutor.");

            //var infosDoctorCreated = await _doctorUseCase.GetDoctorByIdAsync(createdDoctorId); --> Ativar quando existir o método
            //return Ok(infosDoctorCreated);

            return Ok($"Doutor ID {createdDoctorId} criado com sucesso!"); //Remover ao ativar as linhas acima.
        }
    }
}