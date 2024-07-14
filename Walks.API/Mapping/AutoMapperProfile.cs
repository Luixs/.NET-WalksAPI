using AutoMapper;
using Walks.API.Models.Domain;
using Walks.API.Models.DTO;

namespace Walks.API.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<Region, RegionDto>().ReverseMap();
            CreateMap<Region, AddRegionDto>().ReverseMap();
            CreateMap<Walk, WalkDto>().ReverseMap();
            CreateMap<Walk, RequestWalkDto>().ReverseMap();            
            CreateMap<Difficulty, DifficultyDto>().ReverseMap();
        }
    }
}
