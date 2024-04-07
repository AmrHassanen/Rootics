using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Root.API.Data;
using Root.API.Dtos;
using Root.API.Interface;
using Root.API.Models;
using System;
using System.Threading.Tasks;

namespace Root.API.Repositories
{
    public class PlantService : IPlantService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPhotoService _photoService;

        public PlantService(ApplicationDbContext context, IPhotoService photoService)
        {
            _context = context;
            _photoService = photoService;
        }

        public async Task<List<Plant>> GetAllPlants()
        {
            return await _context.Plants.ToListAsync();
        }

        public async Task<Plant> GetPlantById(int plantId)
        {
            return await _context.Plants.FindAsync(plantId);
        }

        public async Task<Plant> AddPlant(PlantDto plantDto)
        {
            if (plantDto.image == null || plantDto.image.Length == 0)
            {
                throw new ArgumentNullException("imageFile", "Image file cannot be null or empty.");
            }

            if (_photoService != null) // Check if _photoService is not null
            {
                var uploadResult = await _photoService.AddPhotoAsync(plantDto.image);

                if (uploadResult.Error != null)
                {
                    throw new Exception(uploadResult.Error.Message);
                }

                var plant = new Plant
                {
                    Name = plantDto.Name,
                    image = uploadResult.SecureUrl.AbsoluteUri,
                    Description = plantDto.Description
                };

                _context.Plants.Add(plant);
                await _context.SaveChangesAsync();

                return plant;
            }
            else
            {
                throw new NullReferenceException("_photoService is not initialized.");
            }
        }


        public async Task<Plant> UpdatePlant(int plantId, PlantDto updatedPlantDto)
        {
            // Get the existing plant from the database
            var existingPlant = await _context.Plants.FindAsync(plantId);

            // If the existing plant is not found, return null
            if (existingPlant == null)
            {
                return null;
            }

            // Update the existing plant entity with the data from the updatedPlantDto
            existingPlant.Name = updatedPlantDto.Name;
            existingPlant.Description = updatedPlantDto.Description;

            // Save changes to the database
            await _context.SaveChangesAsync();

            return existingPlant;
        }
    }
}
