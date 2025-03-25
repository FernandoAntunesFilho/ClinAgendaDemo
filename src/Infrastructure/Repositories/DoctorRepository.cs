using ClinAgendaDemo.src.Application.DTOs.Doctor;
using ClinAgendaDemo.src.Core.Interfaces;
using Dapper;
using MySql.Data.MySqlClient;

namespace ClinAgendaDemo.src.Infrastructure.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly MySqlConnection _connection;

        public DoctorRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> InsertDoctorAsync(DoctorInsertDTO doctor)
        {
            string queryDoctor = @"
            INSERT INTO DOCTOR (NAME, STATUSID)
            VALUES (@Name, @StatusId);
            SELECT LAST_INSERT_ID();";

            string queryDoctorSpecialty = @"
            INSERT INTO DOCTOR_SPECIALTY (DOCTORID, SPECIALTYID)
            VALUES (@DoctorId, @SpecialtyId);";

            var lastDoctorId = await _connection.ExecuteScalarAsync<int>(queryDoctor, doctor);

            foreach (var specialty in doctor.Specialties)
            {
                await _connection.ExecuteScalarAsync<int>(queryDoctorSpecialty,
                    new { doctorId = lastDoctorId, specialtyId = specialty });
            }

            return lastDoctorId;
        }
    }
}