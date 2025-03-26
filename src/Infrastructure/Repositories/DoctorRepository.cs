using System.Text;
using ClinAgendaDemo.src.Application.DTOs.Doctor;
using ClinAgendaDemo.src.Core.Interfaces;
using Dapper;
using MySql.Data.MySqlClient;

namespace ClinAgendaDemo.src.Infrastructure.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly MySqlConnection _connection;
        private readonly IDoctorSpecialtyRepository _doctorSpecialtyRepository;

        public DoctorRepository(MySqlConnection connection, IDoctorSpecialtyRepository doctorSpecialtyRepository)
        {
            _connection = connection;
            _doctorSpecialtyRepository = doctorSpecialtyRepository;
        }

        public async Task<IEnumerable<DoctorListDTO>> GetDoctorAsync(
            string? name,
            int? specialtyId,
            int? statusId,
            int offset,
            int itemsPerPage)
            {
                var innerJoins = new StringBuilder(@"
                FROM DOCTOR D
                INNER JOIN STATUS S ON D.STATUSID = S.ID
                INNER JOIN DOCTOR_SPECIALTY DSPE ON DSPE.DOCTORID = D.ID
                WHERE 1 = 1");

                var parameters = new DynamicParameters();

                if (!string.IsNullOrEmpty(name))
                {
                    innerJoins.Append("AND D.NAME LIKE @Name");
                    parameters.Add("Name", $"%{name}%");
                }

                if (specialtyId.HasValue)
                {
                    innerJoins.Append("AND DSPE.SPECIALTYID = @SpecialtyId");
                    parameters.Add("SpecialtyId", specialtyId.Value);
                }

                if (statusId.HasValue)
                {
                    innerJoins.Append("AND S.ID = @StatusId");
                    parameters.Add("StatusId", statusId.Value);
                }

                parameters.Add("LIMIT", itemsPerPage);
                parameters.Add("PAGE", offset);

                //TODO: Finalizar o método.
            }

        public async Task<int> InsertDoctorAsync(DoctorInsertDTO doctor)
        {
            string queryDoctor = @"
            INSERT INTO DOCTOR (NAME, STATUSID)
            VALUES (@Name, @StatusId);
            SELECT LAST_INSERT_ID();";            

            var lastDoctorId = await _connection.ExecuteScalarAsync<int>(queryDoctor, doctor);

            DoctorSpecialtyDTO doctorSpecialtyDTO = new()
            {
                DoctorId = lastDoctorId,
                SpecialtiesIds = doctor.Specialties
            };

            await _doctorSpecialtyRepository.InsertDoctorSpecialtyAsync(doctorSpecialtyDTO);

            return lastDoctorId;
        }
    }
}