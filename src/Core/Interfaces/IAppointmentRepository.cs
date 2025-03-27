using ClinAgendaDemo.src.Application.DTOs.Appointment;

namespace ClinAgendaDemo.src.Core.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<int> InsertAppointmentAsync(AppointmentInsertDTO appointment);
        Task<bool> UpdateAppointmentAsync(AppointmentUpdateDTO appointment);
    }
}