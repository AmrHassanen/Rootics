using CaptionGenerator.CORE.Settings;
using Root.API.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Root.API.Dtos
{
    public class AdvertisementDto
    {
        public string? PlantDiseaseName { get; set; }
        public string? MedicineName { get; set; }
        public string? CompanyName { get; set; }
        [Required(ErrorMessage = "Image URL is required.")]
        [AllowedExtension(FileSettings.AllowedExtensions),
            maxSizeAllowed(FileSettings.MaxFileSizeInBytes)]
        public IFormFile? TreatmentImage { get; set; }
    }
}
