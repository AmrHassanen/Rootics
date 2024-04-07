using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Root.API.Data;
using Root.API.Models;
using User.Mangement.Service.Services;

namespace Root.API
{
    public class FavoritePlantService : IFavoritePlantService
    {
        private readonly ApplicationDbContext _context;

        public FavoritePlantService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApplicationUser> GetCurrentUserAsync(ClaimsPrincipal user)
        {
            // Retrieve the user from the UserManager using the user's ClaimsPrincipal
            ApplicationUser currentUser = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == user.Identity.Name);

            return currentUser;
        }

        public async Task<bool> AddPlantToFavorites(ApplicationUser user, int plantId)
        {
            // Check if the plant already exists in the user's favorite plants
            bool isAlreadyFavorite = await _context.FavoritePlants
                .AnyAsync(fp => fp.UserId == user.Id && fp.PlantId == plantId);

            if (isAlreadyFavorite)
            {
                // The plant is already in the user's favorite list.
                return false;
            }

            // Create a new FavoritePlant object with the correct UserId
            var favoritePlant = new FavoritePlant
            {
                UserId = user.Id,  // Set the UserId to the Id of the current user
                PlantId = plantId
            };

            // Add the new favorite plant to the context and save changes
            _context.FavoritePlants.Add(favoritePlant);
            await _context.SaveChangesAsync();

            // Plant successfully added to the favorite list.
            return true;
        }
        public async Task<List<FavoritePlant>> GetFavoritePlants(ApplicationUser user)
        {
            // Retrieve all favorite plants for the specified user, including plant information
            List<FavoritePlant> favoritePlants = await _context.FavoritePlants
                .Include(fp => fp.Plant) // Include the related Plant entity
                .Where(fp => fp.UserId == user.Id)
                .ToListAsync();

            return favoritePlants;
        }
    }
}
