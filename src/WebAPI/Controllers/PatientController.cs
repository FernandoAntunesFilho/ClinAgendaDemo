using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinAgendaDemo.src.Application.DTOs.Patient;
using ClinAgendaDemo.src.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace ClinAgendaDemo.src.WebAPI.Controllers
{
    [ApiController]
    [Route("api/patient")]
    public class PatientController : ControllerBase
    {
        private readonly PatientUseCase _patientUseCase;
        public PatientController(PatientUseCase patientUseCase)
        {
            _patientUseCase = patientUseCase;
        }

        [HttpPost("list")]
        public async Task<IActionResult> GetPatientAsync([FromBody] PatientRequestDTO request)
        {
            try
            {
                var patients = await _patientUseCase.GetPatientsAsync(request);
                return Ok(patients);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpGet("list/{id}")]
        public async Task<IActionResult> GetPatientsByIdAsync(int id)
        {
            try
            {
                var patient = await _patientUseCase.GetPetientById(id);
                return Ok(patient);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpPost("insert")]
        public async Task<IActionResult> CreatePatientAsync([FromBody] PatientInsertDTO request)
        {
            try
            {
                var response = await _patientUseCase.CreatePatient(request);
                return Ok($"Paciente id: {response} criado com sucesso");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }
    }
}