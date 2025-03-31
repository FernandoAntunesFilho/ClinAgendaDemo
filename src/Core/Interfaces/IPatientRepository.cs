using ClinAgenda.src.Application.DTOs.Patient;

namespace ClinAgenda.src.Core.Interfaces
{
    public interface IPatientRepository
    {
        Task<(int total, IEnumerable<PatientListDTO> patient)> GetPatientsAsync(string? name, string? documentNumber, int? statusId, int itemsPerPage, int page);
        Task<int> InsertPatientAsync(PatientInsertDTO patient);
        Task<PatientDTO?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(PatientDTO patient);
        Task<bool> DeleteByPatientIdAsync(int patientId);
    }
}