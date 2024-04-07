using Root.API.Dtos;
using Root.API.Models;

namespace Root.API.Interface
{
    public interface ISuggestedPlantService
    {
        Task<List<SuggestedPlant>> GetAllSuggestedPlants();
        Task<SuggestedPlant> AddSuggestedPlant(SuggestedPlantDto suggestedPlantDto);

    }
}
