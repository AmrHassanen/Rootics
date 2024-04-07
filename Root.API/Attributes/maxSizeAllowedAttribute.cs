using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Root.API.Attributes
{
    public class maxSizeAllowedAttribute:ValidationAttribute
    {
        private readonly int _maxSize;
        public maxSizeAllowedAttribute(int maxSize)
        {
            _maxSize = maxSize;
        }
        protected override ValidationResult?
            IsValid(object? value, ValidationContext validationContext)
        {   
            var file = value as IFormFile ;
            if (file is not null) 
            {
                if(file.Length > _maxSize)
                {
                    return new ValidationResult($"Maximum Allowed Size Is {_maxSize/1024/1024} MB .");
                }

            }
            return ValidationResult.Success;
        }
    }
}
