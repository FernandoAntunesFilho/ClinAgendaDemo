namespace ClinAgendaDemo.src.Application.DTOs.Appointment
{
    public class AppointmentInsertDTO
    {
        public required int PatientId { get; set; }
        public required int DoctorId { get; set; }
        public required int SpecialtyId { get; set; }
        public required DateTime ScheduleDate { get; set; }
        public required string Observation { get; set; }
    }
}