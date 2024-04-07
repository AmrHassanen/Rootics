using Root.API.Dtos;
using Root.API.Models;

namespace Root.API.Interface
{
    // IUserPlantActivityService.cs
    public interface IUserPlantActivityService
    {
        Task<int> AddTaskToUser(string userId, UserTaskRequestDto task);
    }

}
