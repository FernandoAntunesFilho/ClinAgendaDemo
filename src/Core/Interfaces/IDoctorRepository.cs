using ClinAgendaDemo.src.Application.DTOs.Doctor;

namespace ClinAgendaDemo.src.Core.Interfaces
{
    public interface IDoctorRepository
    {
        Task<int> InsertDoctorAsync(DoctorInsertDTO doctor);
    }
}