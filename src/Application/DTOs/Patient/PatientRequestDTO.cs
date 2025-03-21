using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinAgendaDemo.src.Application.DTOs.Patient
{
    public class PatientRequestDTO
    {
        public string? Name { get; set; }
        public string? DocumentNumber { get; set; }
        public int? StatusId { get; set; }
    }
}