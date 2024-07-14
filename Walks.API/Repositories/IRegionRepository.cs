using Walks.API.Models.Domain;

namespace Walks.API.Repositories
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsync();

        Task<Region?> GetByIdAsync(Guid id);

        Task<Region> CreateAsync(Region newRegion);

        Task<Region?> UpdateAsync(Guid id, Region newRegion);

        Task<Region?> DeleteAsync(Guid id);
    }
}
