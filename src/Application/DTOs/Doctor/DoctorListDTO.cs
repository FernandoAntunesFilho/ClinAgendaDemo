namespace ClinAgendaDemo.src.Application.DTOs.Doctor
{
    public class DoctorListDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required int StatusId { get; set; }
        public required string StatusName { get; set; }
    }
}