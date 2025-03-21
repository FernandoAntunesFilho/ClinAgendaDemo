using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinAgendaDemo.src.Application.DTOs.Patient;

namespace ClinAgendaDemo.src.Core.Interfaces
{
    public interface IPatientRepository
    {
        Task<PatientListDTO> GetByIdAsync(int id);
        Task<IEnumerable<PatientListDTO>> GetAllAsync(PatientRequestDTO request);
        Task<int> InsertPatientAsync(PatientInsertDTO patientInsertDTO);
    }
}