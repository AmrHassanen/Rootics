using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Root.API.Dtos;
using Root.API.Interface;

namespace Root.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SuggestedPlantController : ControllerBase
    {
        private readonly ISuggestedPlantService _suggestedPlantService;

        public SuggestedPlantController(ISuggestedPlantService suggestedPlantService)
        {
            _suggestedPlantService = suggestedPlantService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSuggestedPlants()
        {
            var suggestedPlants = await _suggestedPlantService.GetAllSuggestedPlants();
            return Ok(suggestedPlants);
        }
        [HttpPost]
        public async Task<IActionResult> AddSuggestedPlant([FromForm]SuggestedPlantDto suggestedPlantDto)
        {
            var suggestedPlant = await _suggestedPlantService.AddSuggestedPlant(suggestedPlantDto);
            return Ok(suggestedPlant);
        }
    }
}
