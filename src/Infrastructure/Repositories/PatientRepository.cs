using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<PatientListDTO> GetByIdAsync(int id)
        {
            const string query = @"
                SELECT
                    id,
                    name,
                    phoneNumber,
                    documentNumber,
                    statusId,
                    birthDate
                    FROM patient;
                WHERE ID = @Id";

            var patients = await _connection.QueryFirstOrDefaultAsync<PatientListDTO>(query, new { Id = id });

            return patients; //TODO: Continuar a implementar aqui.
        }
    }
}