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

        public async Task<DoctorListDTO?> GetDoctorByIdAsync(int id)
        {
            var query = @"
                SELECT 
                    D.ID AS ID,
                    D.NAME AS NAME,
                    D.STATUSID AS STATUSID,
                    S.NAME AS STATUSNAME
                FROM
                    DOCTOR D
                        INNER JOIN
                    STATUS S ON S.ID = D.STATUSID
                WHERE
                    D.ID = @Id;";

            var doctor = await _connection.QueryFirstOrDefaultAsync<DoctorListDTO>(query, new { id });

            return doctor;
        }

        public async Task<(int total, IEnumerable<DoctorListDTO> doctors)> GetDoctorAsync(
            string? name,
            int? specialtyId,
            int? statusId,
            int itemsPerPage,
            int page)
        {
            var innerJoins = new StringBuilder(@"
                 FROM DOCTOR D
                INNER JOIN STATUS S ON D.STATUSID = S.ID
                INNER JOIN DOCTOR_SPECIALTY DSPE ON DSPE.DOCTORID = D.ID
                WHERE 1 = 1");

            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(name))
            {
                innerJoins.Append(" AND D.NAME LIKE @Name");
                parameters.Add("Name", $"%{name}%");
            }

            if (specialtyId.HasValue)
            {
                innerJoins.Append(" AND DSPE.SPECIALTYID = @SpecialtyId");
                parameters.Add("SpecialtyId", specialtyId.Value);
            }

            if (statusId.HasValue)
            {
                innerJoins.Append(" AND S.ID = @StatusId");
                parameters.Add("StatusId", statusId.Value);
            }

            var countQuery = $"SELECT COUNT(DISTINCT D.ID) {innerJoins}";
            int total = await _connection.ExecuteScalarAsync<int>(countQuery, parameters);

            var dataQuery = $@"
                    SELECT DISTINCT
                        D.ID AS ID, 
                        D.NAME AS NAME,
                        D.STATUSID AS STATUSID,
                        S.NAME AS STATUSNAME
                    {innerJoins}
                    ORDER BY D.ID
                    LIMIT @Limit OFFSET @Offset";

            parameters.Add("Limit", itemsPerPage);
            parameters.Add("Offset", (page - 1) * itemsPerPage);

            var doctors = await _connection.QueryAsync<DoctorListDTO>(dataQuery, parameters);

            return (total, doctors);
        }

        public async Task<int> InsertDoctorAsync(DoctorInsertDTO doctor)
        {
            string queryDoctor = @"
            INSERT INTO DOCTOR (NAME, STATUSID)
            VALUES (@Name, @StatusId);
            SELECT LAST_INSERT_ID();";

            var lastDoctorId = await _connection.ExecuteScalarAsync<int>(queryDoctor, doctor);

            DoctorSpecialtyInsertDTO doctorSpecialtyDTO = new()
            {
                DoctorId = lastDoctorId,
                SpecialtiesIds = doctor.Specialties
            };

            await _doctorSpecialtyRepository.InsertDoctorSpecialtyAsync(doctorSpecialtyDTO);

            return lastDoctorId;
        }

        public async Task<bool> UpdateDoctorAsync(DoctorDTO request)
        {
            var query = @"
                UPDATE DOCTOR 
                SET 
                    NAME = @Name,
                    STATUSID = @StatusId
                WHERE
                    id = @Id;
            ";

            int rowsAffected = await _connection.ExecuteAsync(query, request);

            var doctorSpecialtyDeleted = await _doctorSpecialtyRepository.DeleteDoctorSpecialtyAsync(request.Id);
            
            if (doctorSpecialtyDeleted)
            {
                var doctorSpecialty = new DoctorSpecialtyInsertDTO
                {
                    DoctorId = request.Id,
                    SpecialtiesIds = request.Specialties
                };

                await _doctorSpecialtyRepository.InsertDoctorSpecialtyAsync(doctorSpecialty);
            }

            return rowsAffected > 0;            
        }
    }
}