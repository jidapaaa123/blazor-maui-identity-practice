using IdentityPractice.Shared.Models;

namespace IdentityPractice.Shared.Interfaces
{
    public interface IVacationService
    {
        Task<List<VacationDTO>> GetVacationsAsync();
        Task AddVacationAsync(VacationDTO vacation);
        Task DeleteVacationAsync(int id);
    }
}
