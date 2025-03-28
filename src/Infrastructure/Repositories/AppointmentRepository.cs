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

        //TODO: GetAll & GetById

        public async Task<int> InsertAppointmentAsync(AppointmentInsertDTO appointment)
        {
            var query = @"
            INSERT INTO APPOINTMENT (PATIENTID, DOCTORID, SPECIALTYID, APPOINTMENTDATE, OBSERVATION)
            VALUES (@PatientId, @DoctorId, @SpecialtyId, @AppointmentDate, @Observation);
            SELECT LAST_INSERT_ID();
            ";
            return await _connection.ExecuteScalarAsync<int>(query, appointment);
        }

        public async Task<bool> UpdateAppointmentAsync(AppointmentDTO appointment)
        {
            var query = @"
                UPDATE APPOINTMENT 
                SET 
                    PATIENTID = @PatientId,
                    DOCTORID = @DoctorId,
                    SPECIALTYID = @SpecialtyId,
                    APPOINTMENTDATE = @AppointmentDate,
                    OBSERVATION = @Observation
                WHERE
                    ID = @Id;
            ";
            var linhasAfetadas = await _connection.ExecuteAsync(query, appointment);

            return linhasAfetadas > 0;
        }
    }
}