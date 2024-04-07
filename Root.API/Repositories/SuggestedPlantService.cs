using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Root.API.Data;
using Root.API.Dtos;
using Root.API.Interface;
using Root.API.Models;

namespace Root.API.Repositories
{
    public class SuggestedPlantService : ISuggestedPlantService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPhotoService _photoService;


        public SuggestedPlantService(ApplicationDbContext context, IPhotoService photoService)
        {
            _context = context;
            _photoService = photoService;
        }

        public async Task<List<SuggestedPlant>> GetAllSuggestedPlants()
        {
            return await _context.SuggestedPlants.ToListAsync();
        }
        public async Task<SuggestedPlant> AddSuggestedPlant(SuggestedPlantDto suggestedPlantDto)
        {
            if (_photoService != null)
            {
                var uploadResult = await _photoService.AddPhotoAsync(suggestedPlantDto.image!);

                if (uploadResult.Error != null)
                {
                    throw new Exception(uploadResult.Error.Message);
                }
                var suggestedPlant = new SuggestedPlant
                {
                    Name = suggestedPlantDto.Name,
                    image = uploadResult.SecureUrl.AbsoluteUri,
                    Description = suggestedPlantDto.Description
                };

                _context.SuggestedPlants.Add(suggestedPlant);
                await _context.SaveChangesAsync();

                return suggestedPlant;
            }
            else
            {
                throw new NullReferenceException("_photoService is not initialized.");
            }
        }
        
    }
}
