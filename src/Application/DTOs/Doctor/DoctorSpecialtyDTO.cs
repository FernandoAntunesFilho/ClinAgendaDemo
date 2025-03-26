namespace ClinAgendaDemo.src.Application.DTOs.Doctor
{
    public class DoctorSpecialtyDTO
    {
        public required int DoctorId { get; set; }
        public required int[] SpecialtiesIds { get; set; }
    }
}