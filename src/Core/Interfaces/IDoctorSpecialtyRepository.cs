using ClinAgendaDemo.src.Application.DTOs.Doctor;

namespace ClinAgendaDemo.src.Core.Interfaces
{
    public interface IDoctorSpecialtyRepository
    {
        Task<int> InsertDoctorSpecialtyAsync(DoctorSpecialtyDTO doctorSpecialty);
    }
}