using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Root.API.Data;
using Root.API.Interface;
using Root.API.Models;

namespace Root.API.Services
{
    public class UserTaskService : IUserTaskService
    {
        private readonly ApplicationDbContext _context;

        public UserTaskService(ApplicationDbContext context)
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

        public async Task<bool> AddTaskToUserList(ApplicationUser user, int plantId, string taskDescription)
        {
            // Check if the task already exists in the user's task list
            bool isTaskAlreadyAdded = await _context.UserPlantActivities
                .AnyAsync(task => task.UserId == user.Id && task.PlantId == plantId && task.TaskDescription == taskDescription);

            if (isTaskAlreadyAdded)
            {
                // The task is already in the user's task list.
                return false;
            }

            // Create a new UserTask object with the correct UserId
            var userTask = new UserPlantActivity
            {
                UserId = user.Id,  // Set the UserId to the Id of the current user
                PlantId = plantId,
                TaskDescription = taskDescription,
                // Add other properties based on your requirements
            };

            // Add the new task to the context and save changes
            _context.UserPlantActivities.Add(userTask);
            await _context.SaveChangesAsync();

            // Task successfully added to the user's task list.
            return true;
        }
        public async Task<List<UserPlantActivity>> GetUserPlantTasks(ApplicationUser user)
        {
            return await _context.UserPlantActivities
                .Where(activity => activity.UserId == user.Id)
                .Include(activity => activity.Plant) // Include the related Plant entity
                .ToListAsync();
        }

    }
}
