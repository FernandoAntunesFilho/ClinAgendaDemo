using ClinAgenda.src.Application.DTOs.Specialty;
using ClinAgenda.src.Application.DTOs.Status;

namespace ClinAgendaDemo.src.Application.DTOs.Doctor
{
    public class DoctorListReturnDTO
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public IEnumerable<SpecialtyDTO>? Specialty { get; set; }
        public StatusDTO? Status { get; set; }
    }
}