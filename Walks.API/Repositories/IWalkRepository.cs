using Walks.API.Models.Domain;

namespace Walks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsync(Walk newWalk);
        Task<List<Walk>> GetAllAsync();
        Task<Walk?> GetUniqueAsync(Guid id);
        Task<Walk?> UpdateWalkAsync(Guid id, Walk walk);
        Task<Walk?> DeleteWalkAsync(Guid id);
    }
}
