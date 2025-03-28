using ClinAgenda.src.Application.DTOs.Specialty;

namespace ClinAgendaDemo.src.Application.DTOs.Appointment
{
    public class AppointmentListReturnDTO
    {
        public int Id { get; set; }
        public required PatientReturnAppointmentDTO Patient { get; set; }
        public required DoctorReturnAppointmentDTO Doctor { get; set; }
        public required SpecialtyDTO Specialty { get; set; }
        public required DateTime AppointmentDate { get; set; }
    }
}