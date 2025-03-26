using ClinAgendaDemo.src.Application.DTOs.Doctor;
using ClinAgendaDemo.src.Core.Interfaces;
using Dapper;
using MySql.Data.MySqlClient;

namespace ClinAgendaDemo.src.Infrastructure.Repositories
{
    public class DoctorSpecialtyRepository : IDoctorSpecialtyRepository
    {
        private readonly MySqlConnection _connection;

        public DoctorSpecialtyRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<DoctorSpecialtyDTO>> GetDoctorSpecialtyByDoctorId(int[] doctorIds)
        {
            string query = @"
            SELECT 
                DS.DOCTORID AS DOCTORID,
                DS.SPECIALTYID AS SPECIALTYID,
                S.NAME AS SPECIALTYNAME,
                S.SCHEDULEDURATION AS SCHEDULEDURATION
            FROM
                DOCTOR_SPECIALTY DS
                    LEFT JOIN
                SPECIALTY S ON S.ID = DS.SPECIALTYID
            WHERE
                DS.DOCTORID IN @DoctorIds
            ORDER BY DS.DOCTORID;";

            var result = await _connection.QueryAsync<DoctorSpecialtyDTO>(query, new { doctorIds });

            return result;
        }

        public async Task<int> InsertDoctorSpecialtyAsync(DoctorSpecialtyInsertDTO doctorSpecialty)
        {
            string queryDoctorSpecialty = @"
            INSERT INTO DOCTOR_SPECIALTY (DOCTORID, SPECIALTYID)
            VALUES (@DoctorId, @SpecialtyId);";

            foreach (var specialty in doctorSpecialty.SpecialtiesIds)
            {
                await _connection.ExecuteScalarAsync<int>(queryDoctorSpecialty,
                    new { doctorId = doctorSpecialty.DoctorId, specialtyId = specialty });
            }

            return 0;
        }

        public async Task<bool> DeleteDoctorSpecialtyAsync(int doctorId)
        {
            var query = @"
                DELETE FROM DOCTOR_SPECIALTY 
                WHERE
                    DOCTORID = @DoctorId;
            ";

            var rowsAffected = await _connection.ExecuteAsync(query, new { DoctorId = doctorId });

            return rowsAffected > 0;
        }
    }
}