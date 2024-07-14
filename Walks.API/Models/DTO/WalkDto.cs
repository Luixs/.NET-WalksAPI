namespace Walks.API.Models.DTO
{
    public class WalkDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public DifficultyDto Difficulty { get; set; }
        public RegionDto Region { get; set; }
    }

    public class RequestWalkDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double? LengthInKm { get; set; }
        public Guid? DifficultyId { get; set; }
        public Guid? RegionId { get; set; }
    }
}
