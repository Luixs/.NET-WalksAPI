using Walks.API.Models.Domain;

namespace Walks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsync(Walk newWalk);
        Task<List<Walk>> GetAllAsync
        (
            string? filterOn = null, 
            string? filterQuery = null, 
            string? sortBy = null,
            bool isSortAsc = true,
            int pageNumber = 1,
            int pageSize = 1000
        );
        Task<Walk?> GetUniqueAsync(Guid id);
        Task<Walk?> UpdateWalkAsync(Guid id, Walk walk);
        Task<Walk?> DeleteWalkAsync(Guid id);
    }
}
