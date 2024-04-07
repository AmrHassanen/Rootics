using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Root.API.Models;
using User.Mangement.Service.Services;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IFavoritePlantService _favoritePlantService;

    public UserController(IFavoritePlantService favoritePlantService)
    {
        _favoritePlantService = favoritePlantService;
    }

    [HttpPost("AddPlantToFavorites/{plantId}")]
    public async Task<IActionResult> AddPlantToFavorites(int plantId)
    {
        if (plantId <= 0)
        {
            return BadRequest("Invalid plantId");
        }

        // Retrieve the current user
        ApplicationUser currentUser = await _favoritePlantService.GetCurrentUserAsync(HttpContext.User);

        if (currentUser == null)
        {
            return Unauthorized(); // User not authenticated
        }

        bool addedToFavorite = await _favoritePlantService.AddPlantToFavorites(currentUser, plantId);

        if (addedToFavorite)
        {
            return Ok("Plant successfully added to the favorite list.");
        }
        else
        {
            return BadRequest("The plant is already in the user's favorite list.");
        }
    }
    [HttpGet("GetFavoritePlants")]
    public async Task<IActionResult> GetFavoritePlants()
    {
        // Retrieve the current user
        ApplicationUser currentUser = await _favoritePlantService.GetCurrentUserAsync(HttpContext.User);

        if (currentUser == null)
        {
            return Unauthorized(); // User not authenticated
        }

        // Retrieve all favorite plants for the current user
        List<FavoritePlant> favoritePlants = await _favoritePlantService.GetFavoritePlants(currentUser);

        return Ok(favoritePlants);
    }
}
