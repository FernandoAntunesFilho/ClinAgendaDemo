using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinAgendaDemo.src.Application.DTOs.Patient;
using ClinAgendaDemo.src.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace ClinAgendaDemo.src.WebAPI.Controllers
{
    public class PatientController : ControllerBase
    {
        private readonly PatientUseCase _patientUseCase;
        public PatientController(PatientUseCase patientUseCase)
        {
            _patientUseCase = patientUseCase;
        }

        [HttpPost("list")]
        public async Task<IActionResult> GetSpecialtyAsync([FromBody] PatientRequestDTO request)
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
    }
}