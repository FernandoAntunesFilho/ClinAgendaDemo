using ClinAgendaDemo.src.Application.DTOs.Appointment;

namespace ClinAgendaDemo.src.Core.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<int> InsertAppointmentAsync(AppointmentInsertDTO appointment);
        Task<bool> UpdateAppointmentAsync(AppointmentDTO appointment);
        Task<(int total, IEnumerable<AppointmentListDTO>)> GetAppointmentsAsync(
            string? patientName, string? doctorName, int? specialtyId, int itemsPerPage, int page);
        Task<AppointmentDTO?> GetAppointmentByIdAsync(int id);
    }
}