using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinAgendaDemo.src.Application.DTOs.Patient;
using ClinAgendaDemo.src.Core.Interfaces;

namespace ClinAgendaDemo.src.Application.UseCases
{
    public class PatientUseCase
    {
        private readonly IPatientRepository _patientRepository;

        public PatientUseCase(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<object> GetPatientsAsync(PatientRequestDTO request)
        {
            return await _patientRepository.GetAllAsync(request);            
        }

        public async Task<PatientListDTO> GetPetientById(int id)
        {
            return await _patientRepository.GetByIdAsync(id);
        }

        public async Task<int> CreatePatient(PatientInsertDTO request)
        {
            return await _patientRepository.InsertPatientAsync(request);
        }
    }
}