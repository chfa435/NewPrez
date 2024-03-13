using NewTiceAI.Models;

namespace NewTiceAI.Services.Interfaces
{
    public interface IInstitutionService
    {
        public Task AddInstitutionAsync(Institution? institution);

        public Task UpdateInstitutionAsync(Institution? institution);

        public Task DeleteInstitutionAsync(Institution? institution);

        public Task<Institution> GetInstitutionAsync(int? institutionId);

        public Task<List<Institution>> GetInstitutionsAsync();

    }
}
