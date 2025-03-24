using ClinAgenda.src.Application.DTOs.Specialty;

namespace ClinAgenda.src.Core.Interfaces
{
    public interface ISpecialtyRepository
    {
        Task<(int total, IEnumerable<SpecialtyDTO> specialtys)> GetAllAsync(int? itemsPerPage, int? page);
        Task<int> InsertSpecialtyAsync(SpecialtyInsertDTO specialtyInsertDTO);
        Task<SpecialtyDTO> GetByIdAsync(int id);
        Task<int> DeleteSpecialtyAsync(int id);
    }
}