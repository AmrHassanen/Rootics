using Root.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace User.Mangement.Service.Services
{
    public interface IFavoritePlantService
    {
        Task<ApplicationUser> GetCurrentUserAsync(ClaimsPrincipal user);
        Task<bool> AddPlantToFavorites(ApplicationUser user, int plantId);
        Task<List<FavoritePlant>> GetFavoritePlants(ApplicationUser user);

    }
}
