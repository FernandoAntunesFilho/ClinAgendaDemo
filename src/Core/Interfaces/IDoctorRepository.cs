using ClinAgendaDemo.src.Application.DTOs.Doctor;

namespace ClinAgendaDemo.src.Core.Interfaces
{
    public interface IDoctorRepository
    {
        Task<(int total, IEnumerable<DoctorListDTO> doctors)> GetDoctorAsync(
            string? name,
            int? specialtyId,
            int? statusId,
            int itemsPerPage,
            int page);

        Task<int> InsertDoctorAsync(DoctorInsertDTO doctor);

        Task<DoctorListDTO?> GetDoctorByIdAsync(int id);

        Task<bool> UpdateDoctorAsync(DoctorDTO request);
    }
}