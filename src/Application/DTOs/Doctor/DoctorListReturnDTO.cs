using ClinAgenda.src.Application.DTOs.Specialty;
using ClinAgenda.src.Application.DTOs.Status;

namespace ClinAgendaDemo.src.Application.DTOs.Doctor
{
    public class DoctorListReturnDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required IEnumerable<SpecialtyDTO> Specialty { get; set; }
        public required StatusDTO Status { get; set; }
    }
}