using Microsoft.EntityFrameworkCore;
using Root.API.Data;
using Root.API.Dtos;
using Root.API.Interface;
using Root.API.Models;

namespace Root.API.Repositories
{
    // UserPlantActivityService.cs
    public class UserPlantActivityService : IUserPlantActivityService
    {
        private readonly ApplicationDbContext _dbContext;

        public UserPlantActivityService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddTaskToUser(string userId, UserTaskRequestDto taskDto)
        {
            // Ensure that the user exists
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                // Handle the case where the user does not exist
                return -1; // You can define your own error codes
            }

            // Create a new plant or find an existing one by PlantId
            var plant = _dbContext.Plants.FirstOrDefault(p => p.PlantId == taskDto.PlantId);
            if (plant == null)
            {
                // Handle the case where the plant does not exist
                return -2; // You can define your own error codes
            }

            // Map DTO properties to the UserPlantActivity model
            var task = new UserPlantActivity
            {
                UserId = userId,
                PlantId = plant.PlantId,
                TaskDescription = taskDto.TaskDescription,
                // Add other properties as needed
            };

            _dbContext.UserPlantActivities.Add(task);
            await _dbContext.SaveChangesAsync();

            return task.UserPlantActivityId;
        }
    }

}
