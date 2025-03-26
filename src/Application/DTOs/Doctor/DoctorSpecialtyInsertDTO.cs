namespace ClinAgendaDemo.src.Application.DTOs.Doctor
{
    public class DoctorSpecialtyInsertDTO
    {
        public required int DoctorId { get; set; }
        public required int[] SpecialtiesIds { get; set; }
    }
}