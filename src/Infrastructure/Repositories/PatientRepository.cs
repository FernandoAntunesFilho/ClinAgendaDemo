using System.Text;
using ClinAgenda.src.Application.DTOs.Status;
using ClinAgendaDemo.src.Application.DTOs.Patient;
using ClinAgendaDemo.src.Core.Interfaces;
using Dapper;
using MySql.Data.MySqlClient;

namespace ClinAgendaDemo.src.Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly MySqlConnection _connection;

        public PatientRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<PatientListDTO>> GetAllAsync(PatientRequestDTO request)
        {
            var queryBase = new StringBuilder(@"
                FROM patient p
                INNER JOIN status s
                ON p.statusId = s.id
                where 1 = 1");

            var parameters = new DynamicParameters();

            var dataQuery = $@"
                SELECT
                    p.id,
                    p.name,
                    p.phoneNumber,
                    p.documentNumber,
                    p.statusId,
                    p.birthDate,
                    s.id,
                    s.name 
                    {queryBase}";

            if (!string.IsNullOrEmpty(request.Name))
            {
                dataQuery += " and p.name = @Name";
                parameters.Add("@Name", request.Name);
            }

            if (!string.IsNullOrEmpty(request.DocumentNumber))
            {
                dataQuery += " and p.documentNumber = @DocumentNumber";
                parameters.Add("@DocumentNumber", request.DocumentNumber);
            }

            if (request.StatusId.HasValue)
            {
                dataQuery += " and p.statusId = @StatusId";
                parameters.Add("@StatusId", request.StatusId);
            }

            var patients = await _connection.QueryAsync<PatientListDTO, StatusDTO, PatientListDTO>(dataQuery,
                (patient, status) =>
                {
                    patient.Status = status;
                    return patient;
                },
                parameters,
                splitOn: "Id"
            );

            return patients;
        }

        public async Task<PatientListDTO?> GetByIdAsync(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("id", id);

            var query = $@"
                SELECT
                    p.id,
                    p.name,
                    p.phoneNumber,
                    p.documentNumber,
                    p.statusId,
                    p.birthDate,
                    s.id,
                    s.name 
                    FROM patient p
                INNER JOIN status s
                ON p.statusId = s.id
                where p.id = @id";

            var patient = (await _connection.QueryAsync<PatientListDTO, StatusDTO, PatientListDTO>(
                query,
                (patient, status) =>
                {
                    patient.Status = status;
                    return patient;
                },
                parameters,
                splitOn: "id"
            )).FirstOrDefault();

            return patient;
        }

        public async Task<int> InsertPatientAsync(PatientInsertDTO request)
        {
            string query = @"
                INSERT INTO PATIENT (NAME, PHONENUMBER, DOCUMENTNUMBER, STATUSID, BIRTHDATE)
                VALUES (@Name, @PhoneNumber, @DocumentNumber, @StatusId, @BirthDate);
                SELECT LAST_INSERT_ID();";
            
            return await _connection.ExecuteScalarAsync<int>(query, request);
        }

        public async Task<int> UpdatePatientAsync(PatientDTO request)
        {
            string query = @"
            UPDATE patient
                SET name = @Name,
                phoneNumber = @PhoneNumber,
                documentNumber = @DocumentNumber,
                statusId = @StatusId,
                birthDate = @BirthDate
            WHERE id = @Id;
            ";

            return await _connection.ExecuteScalarAsync<int>(query, request);
        }

        public async Task<int> DeletePatientAsync(int id)
        {
            string query = @"
            DELETE FROM patient WHERE id = @Id;
            ";

            return await _connection.ExecuteAsync(query, new { Id = id});
        }
    }
}