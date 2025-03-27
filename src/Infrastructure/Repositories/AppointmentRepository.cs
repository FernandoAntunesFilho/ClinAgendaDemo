using ClinAgendaDemo.src.Application.DTOs.Appointment;
using ClinAgendaDemo.src.Core.Interfaces;
using Dapper;
using MySql.Data.MySqlClient;

namespace ClinAgendaDemo.src.Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly MySqlConnection _connection;

        public AppointmentRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> InsertAppointmentAsync(AppointmentInsertDTO appointment)
        {
            var query = @"
            INSERT INTO APPOINTMENT (PATIENTID, DOCTORID, SPECIALTYID, APPOINTMENTDATE, OBSERVATION)
            VALUES (@PatientId, @DoctorId, @SpecialtyId, @ScheduleDate, @Observation);
            SELECT LAST_INSERT_ID();
            ";
            return await _connection.ExecuteScalarAsync<int>(query, appointment);
        }

        public Task<bool> UpdateAppointmentAsync(AppointmentUpdateDTO appointment)
        {
            throw new NotImplementedException();
        }
    }
}