namespace ClinAgendaDemo.src.Application.DTOs.Doctor
{
    public class DoctorSpecialtyDTO
    {
        public int DoctorId { get; set; }
        public int SpecialtyId { get; set; }
        public required string SpecialtyName { get; set; }
        public required int ScheduleDuration { get; set; } = 1;
    }
}