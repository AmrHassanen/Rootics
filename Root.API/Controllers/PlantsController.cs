using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Root.API.Dtos;
using Root.API.Interface;
using System.Threading.Tasks;
using User.Mangement.Service.Services;

[ApiController]
[Route("[controller]")]
[Authorize]
public class PlantController : ControllerBase
{
    private readonly IPlantService _plantService;

    public PlantController(IPlantService plantService)
    {
        _plantService = plantService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPlants()
    {
        var plants = await _plantService.GetAllPlants();
        return Ok(plants);
    }

    [HttpGet("{plantId}")]
    public async Task<IActionResult> GetPlantById(int plantId)
    {
        var plant = await _plantService.GetPlantById(plantId);

        if (plant == null)
        {
            return NotFound();
        }

        return Ok(plant);
    }
    [HttpPost]
    public async Task<IActionResult> AddPlant([FromForm] PlantDto plantDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var addedPlant = await _plantService.AddPlant(plantDto);
            return CreatedAtAction(nameof(GetPlantById), new { plantId = addedPlant.PlantId }, addedPlant);
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            return StatusCode(500, "Internal server error");
        }
    }
    [HttpPut("{plantId}")]
    public async Task<IActionResult> UpdatePlant(int plantId, [FromForm] PlantDto updatedPlantDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var updatedPlant = await _plantService.UpdatePlant(plantId, updatedPlantDto);

            if (updatedPlant == null)
            {
                return NotFound();
            }

            return Ok(updatedPlant);
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            return StatusCode(500, "Internal server error");
        }
    }

}
