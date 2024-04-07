using CaptionGenerator.CORE.Settings;
using Root.API.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Root.API.Dtos
{
    public class PlantDto
    {
        public string? Name { get; set; }
        [Required(ErrorMessage = "Image URL is required.")]
        [AllowedExtension(FileSettings.AllowedExtensions),
            maxSizeAllowed(FileSettings.MaxFileSizeInBytes)]
        public IFormFile ? image { get; set; }
        public string? Description { get; set; }
    }
}
