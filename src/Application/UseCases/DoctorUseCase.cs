using ClinAgendaDemo.src.Application.DTOs.Doctor;
using ClinAgendaDemo.src.Core.Interfaces;

namespace ClinAgendaDemo.src.Application.UseCases
{
    public class DoctorUseCase
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorUseCase(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<int> CreateDoctorAsync(DoctorInsertDTO doctorDTO)
        {
            var newDoctorId = await _doctorRepository.InsertDoctorAsync(doctorDTO);
            return newDoctorId;
        }
    }
}