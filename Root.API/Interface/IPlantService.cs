using Root.API.Dtos;
using Root.API.Models;

namespace Root.API.Interface
{
    public interface IPlantService
    {
        Task<List<Plant>> GetAllPlants();
        Task<Plant> GetPlantById(int plantId);
        Task<Plant> AddPlant(PlantDto plant); // Add this method
        Task<Plant> UpdatePlant(int plantId, PlantDto updatedPlantDto);

    }
}
