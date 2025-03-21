using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinAgendaDemo.src.Application.DTOs.Patient
{
    public class PatientInsertDTO
    {
        public required string Name { get; set; }
        public required string PhoneNumber { get; set; }
        public required string DocumentNumber { get; set; }
        public int StatusId { get; set; }
        public DateTime BirthDate { get; set; }
    }
}