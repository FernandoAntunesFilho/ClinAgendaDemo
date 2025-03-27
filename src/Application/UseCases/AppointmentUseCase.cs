using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinAgendaDemo.src.Application.DTOs.Appointment;
using ClinAgendaDemo.src.Core.Interfaces;

namespace ClinAgendaDemo.src.Application.UseCases
{
    public class AppointmentUseCase
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentUseCase(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<int> Create(AppointmentInsertDTO appointment)
        {
            var newAppointment = await _appointmentRepository.InsertAppointmentAsync(appointment);
            return newAppointment;
        }
    }
}