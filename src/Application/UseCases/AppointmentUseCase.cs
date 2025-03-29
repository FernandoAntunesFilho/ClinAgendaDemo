using ClinAgenda.src.Application.DTOs.Specialty;
using ClinAgenda.src.Application.UseCases;
using ClinAgendaDemo.src.Application.DTOs.Appointment;
using ClinAgendaDemo.src.Core.Interfaces;

namespace ClinAgendaDemo.src.Application.UseCases
{
    public class AppointmentUseCase
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly PatientUseCase _patientUseCase;
        private readonly DoctorUseCase _doctorUseCase;
        private readonly SpecialtyUseCase _specialtyUseCase;

        public AppointmentUseCase(
            IAppointmentRepository appointmentRepository,
            PatientUseCase patientUseCase,
            DoctorUseCase doctorUseCase,
            SpecialtyUseCase specialtyUseCase)
        {
            _appointmentRepository = appointmentRepository;
            _patientUseCase = patientUseCase;
            _doctorUseCase = doctorUseCase;
            _specialtyUseCase = specialtyUseCase;
        }

        public async Task<object> GetAll(
            string? patientName, string? doctorName, int? specialtyId, int itemsPerPage, int page)
        {
            var (total, rawData) = await _appointmentRepository.GetAppointmentsAsync(
                patientName, doctorName, specialtyId, itemsPerPage, page
            );

            var appointments = rawData.Select(a => new AppointmentListReturnDTO
            {
                Id = a.Id,
                Patient = new PatientReturnAppointmentDTO
                {
                    Name = a.PatientName,
                    DocumentNumber = a.DoctorName
                },
                Doctor = new DoctorReturnAppointmentDTO
                {
                    Name = a.DoctorName
                },
                Specialty = new SpecialtyDTO
                {
                    Id = a.SpecialtyId,
                    Name = a.SpecialtyName,
                    ScheduleDuration = a.ScheduleDuration
                },
                AppointmentDate = a.ScheduleDate
            }).ToList();

            return new { total, items = appointments };
        }

        public async Task<AppointmentDTO?> GetPatientByIdAsync(int id)
        {
            return await _appointmentRepository.GetAppointmentByIdAsync(id);
        }

        public async Task<int> Create(AppointmentInsertDTO appointment)
        {
            var hasPatient = await _patientUseCase.GetPatientByIdAsync(appointment.PatientId);
            if (hasPatient == null) throw new KeyNotFoundException($"O Paciente ID {appointment.PatientId} não existe");

            var hasDoctor = await _doctorUseCase.GetDoctorByIdAsync(appointment.DoctorId);
            if (hasDoctor == null) throw new KeyNotFoundException($"O Doutor ID {appointment.DoctorId} não existe");

            var hasSpecialty = await _specialtyUseCase.GetSpecialtyByIdAsync(appointment.SpecialtyId);
            if (hasSpecialty == null) throw new KeyNotFoundException($"A especalidade ID {appointment.SpecialtyId} não existe");

            var newAppointment = await _appointmentRepository.InsertAppointmentAsync(appointment);
            return newAppointment;
        }

        public async Task<bool> Update(int id, AppointmentUpdateDTO appointment)
        {
            var hasPatient = await _patientUseCase.GetPatientByIdAsync(appointment.PatientId);
            if (hasPatient == null) throw new KeyNotFoundException($"O Paciente ID {appointment.PatientId} não existe");

            var hasDoctor = await _doctorUseCase.GetDoctorByIdAsync(appointment.DoctorId);
            if (hasDoctor == null) throw new KeyNotFoundException($"O Doutor ID {appointment.DoctorId} não existe");

            var hasSpecialty = await _specialtyUseCase.GetSpecialtyByIdAsync(appointment.SpecialtyId);
            if (hasSpecialty == null) throw new KeyNotFoundException($"A especalidade ID {appointment.SpecialtyId} não existe");

            var updatedAppointment = new AppointmentDTO
            {
                Id = id,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                SpecialtyId = appointment.SpecialtyId,
                AppointmentDate = appointment.AppointmentDate,
                Observation = appointment.Observation
            };

            var appointmentUpdated = await _appointmentRepository.UpdateAppointmentAsync(updatedAppointment);

            return appointmentUpdated;
        }
    }
}