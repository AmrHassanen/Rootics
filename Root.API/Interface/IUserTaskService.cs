using Root.API.Models;
using System.Security.Claims;

namespace Root.API.Interface
{
    public interface IUserTaskService
    {
        Task<ApplicationUser> GetCurrentUserAsync(ClaimsPrincipal user);
        Task<bool> AddTaskToUserList(ApplicationUser user, int plantId, string taskDescription);
        Task<List<UserPlantActivity>> GetUserPlantTasks(ApplicationUser user);
    }
}
