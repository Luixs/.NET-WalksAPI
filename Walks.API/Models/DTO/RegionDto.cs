using System.ComponentModel.DataAnnotations;

namespace Walks.API.Models.DTO
{
    public class RegionDto
    {
        public Guid Id { get; set; }


        [MinLength(3, ErrorMessage = "Code has to be a minimun of 3 characters")]
        [MaxLength(3, ErrorMessage = "Code has to be a maximum of 3 characters")]
        public string Code { get; set; }

        public string Name { get; set; }

        public string? RegionImageUrl { get; set; }
    }

    public class AddRegionDto
    {
        //[Required] In my case, not necessary this Anotation
        [MinLength(3, ErrorMessage ="Code has to be a minimun of 3 characters")]
        [MaxLength(3, ErrorMessage ="Code has to be a maximum of 3 characters")]
        public string? Code { get; set; }

        public string? Name { get; set; }

        public string? RegionImageUrl { get; set; }
    }

}
