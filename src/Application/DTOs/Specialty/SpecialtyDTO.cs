namespace ClinAgenda.src.Application.DTOs.Specialty
{
    public class SpecialtyDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int ScheduleDuration { get; set; }
    }
}