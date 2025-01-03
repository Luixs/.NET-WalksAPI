﻿using Microsoft.EntityFrameworkCore;
using Walks.API.Data;
using Walks.API.Models.Domain;
using Walks.API.Utils;

namespace Walks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {        
        private readonly WalksDbContext _context;

        public SQLWalkRepository(WalksDbContext walskDbContext)
        {
            _context = walskDbContext;
        }

        public async Task<Walk> CreateAsync(Walk newWalk)
        {
            await _context.Walks.AddAsync(newWalk);
            await _context.SaveChangesAsync();
            return newWalk;
        }

        public async Task<Walk?> DeleteWalkAsync(Guid id)
        {
            var deletedWalk = await _context.Walks.Include("Region").Include("Difficulty").FirstOrDefaultAsync(x => x.Id == id);
            if(deletedWalk == null) return null;

            _context.Walks.Remove(deletedWalk);
            await _context.SaveChangesAsync();

            return deletedWalk;
        }

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isSortAsc = true, int pageNumber = 1, int pageSize = 1000)
        {
            var walks = _context.Walks.Include("Region").Include("Difficulty").AsQueryable();

            // --- Filtering
            if(!String.IsNullOrEmpty(filterOn) && !String.IsNullOrEmpty(filterQuery))
            {
                // --- Filter "Name"
                if(filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where((x => x.Name.Contains(filterQuery)));
                }
            }

            // --- Sorting
            if (!String.IsNullOrEmpty(sortBy))
            {
                // --- Sorting by "Name"
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isSortAsc ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }

                // --- Sorting by "Length"
                if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isSortAsc ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }
            }

            // --- Pagination
            var skipResults = (pageNumber - 1) * pageSize;

            return await walks.Skip(skipResults).Take(pageSize).ToListAsync();
        }
        public async Task<Walk?> GetUniqueAsync(Guid id)
        {
            var walk = await _context.Walks.Include("Region").Include("Difficulty").FirstOrDefaultAsync(x=> x.Id == id);
            return walk;
        }
        public async Task<Walk?> UpdateWalkAsync(Guid id, Walk walk)
        {
            var existWalk = await _context.Walks.FirstOrDefaultAsync(x=> x.Id == id);
            if (existWalk == null || walk == null) return null;


            existWalk.Name = walk?.Name ?? existWalk.Name;
            existWalk.Description = walk?.Description ?? existWalk.Description;
            existWalk.LengthInKm = walk?.LengthInKm ?? existWalk.LengthInKm;
            existWalk.DifficultyId = walk.DifficultyId.ToString().Equals(GlobalConstants.NULLABLE_GUID) ? existWalk.DifficultyId : walk.DifficultyId;
            existWalk.RegionId = walk.RegionId.ToString().Equals(GlobalConstants.NULLABLE_GUID) ? existWalk.RegionId : walk.RegionId;

            await _context.SaveChangesAsync();

            return existWalk;

        }
    }
}
