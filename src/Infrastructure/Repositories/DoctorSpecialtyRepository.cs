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

        public async Task<int> InsertDoctorSpecialtyAsync(DoctorSpecialtyDTO doctorSpecialty)
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
    }
}