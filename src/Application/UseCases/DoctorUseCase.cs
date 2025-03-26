using System.Collections;
using ClinAgenda.src.Application.DTOs.Specialty;
using ClinAgenda.src.Application.DTOs.Status;
using ClinAgendaDemo.src.Application.DTOs.Doctor;
using ClinAgendaDemo.src.Core.Interfaces;

namespace ClinAgendaDemo.src.Application.UseCases
{
    public class DoctorUseCase
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IDoctorSpecialtyRepository _doctorSpecialtyRepository;

        public DoctorUseCase(IDoctorRepository doctorRepository, IDoctorSpecialtyRepository doctorSpecialtyRepository)
        {
            _doctorRepository = doctorRepository;
            _doctorSpecialtyRepository = doctorSpecialtyRepository;
        }

        public async Task<DoctorListReturnDTO?> GetDoctorByIdAsync(int id)
        {
            var doctor = await _doctorRepository.GetDoctorByIdAsync(id);
            if (doctor == null) return null;

            var specialties = await _doctorSpecialtyRepository.GetDoctorSpecialtyByDoctorId([doctor.Id]);

            var response = new DoctorListReturnDTO
            {
                Id = doctor.Id,
                Name = doctor.Name,
                Status = new StatusDTO
                {
                    Id = doctor.StatusId,
                    Name = doctor.StatusName
                },
                Specialty = specialties.Where(s => s.DoctorId == doctor.Id)
                .Select(s => new SpecialtyDTO
                {
                    Id = s.SpecialtyId,
                    Name = s.SpecialtyName,
                    ScheduleDuration = s.ScheduleDuration
                }).ToList()
            };

            return response;
        }

        public async Task<object> GetDoctorsAsync(string? name, int? specialtyId, int? statusId, int itemsPerPage, int page)
        {
            var (total, rawData) = await _doctorRepository.GetDoctorAsync(name, specialtyId, statusId, itemsPerPage, page);

            var doctorsIds = rawData.Select(d => d.Id).ToArray();
            var specialties = await _doctorSpecialtyRepository.GetDoctorSpecialtyByDoctorId(doctorsIds);

            var doctors = rawData
            .Select(d => new DoctorListReturnDTO
            {
                Id = d.Id,
                Name = d.Name,
                Status = new StatusDTO
                {
                    Id = d.StatusId,
                    Name = d.StatusName
                },
                Specialty = specialties.Where(s => s.DoctorId == d.Id)
                .Select(s => new SpecialtyDTO
                {
                    Id = s.SpecialtyId,
                    Name = s.SpecialtyName,
                    ScheduleDuration = s.ScheduleDuration
                }).ToList()
            }).ToList();

            return new { total, items = doctors };
        }

        public async Task<DoctorListReturnDTO> CreateDoctorAsync(DoctorInsertDTO doctorDTO)
        {
            var newDoctorId = await _doctorRepository.InsertDoctorAsync(doctorDTO);
            var newDoctor = await GetDoctorByIdAsync(newDoctorId);
            return newDoctor!;
        }

        public async Task<bool> UpdateDoctorAsync(int id, DoctorUpdateDTO request)
        {
            var exitingDoctor = _doctorRepository.GetDoctorByIdAsync(id);
            if (exitingDoctor == null) throw new KeyNotFoundException("Doutor não encontrado.");

            var doctor = new DoctorDTO
            {
                Id = id,
                Name = request.Name,
                Specialties = request.Specialties,
                StatusId = request.StatusId
            };

            var isUpdated = await _doctorRepository.UpdateDoctorAsync(doctor);
            return isUpdated;
            //TODO: Fazer Controller.
        }
    }
}