using System.Text;
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

        //TODO: GetById
        public async Task<(int total, IEnumerable<AppointmentListDTO>)> GetAppointmentsAsync(
            string? patientName, string? doctorName, int? specialtyId, int itemsPerPage, int page)
        {
            var innerJoins = new StringBuilder(@"
                FROM
                    APPOINTMENT A
                        INNER JOIN
                    PATIENT P ON P.ID = A.PATIENTID
                        INNER JOIN
                    DOCTOR D ON D.ID = A.DOCTORID
                        INNER JOIN
                    SPECIALTY S ON S.ID = A.SPECIALTYID
                WHERE 1 = 1");

            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(patientName))
            {
                innerJoins.Append(" AND P.NAME LIKE @PatientName");
                parameters.Add("PatientName", $"%{patientName}%");
            }

            if (!string.IsNullOrEmpty(doctorName))
            {
                innerJoins.Append(" AND D.NAME LIKE @DoctorName");
                parameters.Add("DoctorName", $"%{doctorName}%");
            }

            if (specialtyId.HasValue)
            {
                innerJoins.Append(" AND S.ID = @SpecialtyId");
                parameters.Add("SpecialtyId", $"%{specialtyId}%");
            }

            var countQuery = $"SELECT COUNT(DISTINCT D.ID) {innerJoins}";
            int total = await _connection.ExecuteScalarAsync<int>(countQuery, parameters);

            var dataQuery = $@"
                SELECT 
                    A.ID AS ID,
                    P.NAME AS PATIENTNAME,
                    P.DOCUMENTNUMBER AS PATIENTDOCUMENTNUMBER,
                    D.NAME AS DOCTORNAME,
                    S.ID AS SPECIALTYID,
                    S.NAME AS SPECIALTYNAME,
                    S.SCHEDULEDURATION AS SCHEDULEDURATION,
                    A.APPOINTMENTDATE AS SCHEDULEDATE 
                {innerJoins}
                    ORDER BY A.ID
                    LIMIT @Limit OFFSET @Offset";

            parameters.Add("Limit", itemsPerPage);
            parameters.Add("Offset", (page - 1) * itemsPerPage);

            var appointments = await _connection.QueryAsync<AppointmentListDTO>(dataQuery, parameters);

            return (total, appointments);
        }

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