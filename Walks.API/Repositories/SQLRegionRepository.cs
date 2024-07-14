using Microsoft.EntityFrameworkCore;
using Walks.API.Data;
using Walks.API.Models.Domain;
using Walks.API.Models.DTO;

namespace Walks.API.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly WalksDbContext _dbContext;
        public SQLRegionRepository(WalksDbContext walksDbContext)
        {
            this._dbContext = walksDbContext;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await _dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {

            // -- This method only call from the ID by default.
            // We can use that sintax when we gonna search by Id.
            // CODE: var region = _dbContext.Regions.Find(id);

            return await _dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await _dbContext.Regions.AddAsync(region);
            await _dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            // try to catch this Region
            var existingRegion = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (existingRegion == null) return null;

            // --- Make the changes
            existingRegion.Code = region?.Code ?? existingRegion.Code;
            existingRegion.Name = region?.Name ?? existingRegion.Name;
            existingRegion.RegionImageUrl = region?.RegionImageUrl ?? existingRegion.RegionImageUrl;

            await _dbContext.SaveChangesAsync();

            return region;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            // try to catch this Region
            var existingRegion = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (existingRegion == null) return null;

            // --- Delete a region
            _dbContext.Regions.Remove(existingRegion);
            await _dbContext.SaveChangesAsync();

            return existingRegion;

        }

    }
}
