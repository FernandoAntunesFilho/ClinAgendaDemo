namespace ClinAgendaDemo.src.Application.DTOs.Appointment
{
    public class AppointmentListDTO
    {
        public required int Id { get; set; }
        public required string PatientName { get; set; }
        public required string PatientDocumentNumber { get; set; }
        public required string DoctorName { get; set; }
        public required int SpecialtyId { get; set; }
        public required string SpecialtyName { get; set; }
        public required int ScheduleDuration { get; set; }
        public required DateTime ScheduleDate { get; set; }
    }
}