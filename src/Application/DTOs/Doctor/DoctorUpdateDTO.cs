using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinAgendaDemo.src.Application.DTOs.Doctor
{
    public class DoctorUpdateDTO
    {
        public required string Name { get; set; }
        public required int[] Specialties { get; set; }
        public required int StatusId { get; set; }
    }
}