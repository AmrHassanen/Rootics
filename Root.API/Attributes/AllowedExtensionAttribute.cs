using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Root.API.Attributes
{
    public class AllowedExtensionAttribute:ValidationAttribute
    {
        private readonly string _allowedExtenions;
        public AllowedExtensionAttribute(string allowedExtenions) 
        {
            _allowedExtenions = allowedExtenions;
        }

        protected override ValidationResult? 
            IsValid(object? value, ValidationContext validationContext)
        {
            var file =value as IFormFile; 
            if (file is not null) 
            {
                var extension = Path.GetExtension(file.FileName);

                var isAllowed = _allowedExtenions.Split(',').Contains(extension,StringComparer.OrdinalIgnoreCase);

                if (!isAllowed) 
                {
                    return new ValidationResult($"Only {_allowedExtenions} Are Allowed!");
                }                    
            }
            return ValidationResult.Success;
        }
    }
}
