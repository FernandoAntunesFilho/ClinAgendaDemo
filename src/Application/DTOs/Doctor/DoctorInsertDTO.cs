namespace ClinAgendaDemo.src.Application.DTOs.Doctor
{
    public class DoctorInsertDTO
    {
        public required string Name { get; set; }
        public required int[] Specialties { get; set; }
        public int StatusId { get; set; }
    }
}