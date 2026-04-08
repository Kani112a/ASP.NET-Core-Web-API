using System.ComponentModel.DataAnnotations;

namespace Cityinfo.API.Model
{
    public class pointOfInterestCreationDto
    {
        [Required(ErrorMessage ="You should provide a Name")]
        [MaxLength(50)]
        public string? Name { get; set; }
        [MaxLength(200)]
        public string? Description { get; set; }
    }
}
