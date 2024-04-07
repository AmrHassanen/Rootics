using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Root.API.Dtos;
using Root.API.Interface;
using Root.API.Models;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UserTasksController : ControllerBase
{
    private readonly IUserTaskService _userTaskService;
    //private readonly IUserPlantActivityService _userPlantActivityService;

    public UserTasksController(IUserTaskService userTaskService, IUserPlantActivityService userPlantActivityService)
    {
        _userTaskService = userTaskService;
       // _userPlantActivityService = userPlantActivityService;

    }

    [HttpPost("addTask")]
    public async Task<IActionResult> AddTaskToUserList([FromForm] UserTaskRequestDto taskDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userTaskService.GetCurrentUserAsync(User);

        if (user == null)
        {
            return Unauthorized(); // User not authenticated
        }

        bool addedToTaskList = await _userTaskService.AddTaskToUserList(user, taskDto.PlantId, taskDto.TaskDescription);

        if (addedToTaskList)
        {
            return Ok("Task successfully added to the user's task list.");
        }
        else
        {
            return BadRequest("The task is already in the user's task list.");
        }
    }

    [HttpGet("getUserPlantTasks")]
    public async Task<IActionResult> GetUserPlantTasks()
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userTaskService.GetCurrentUserAsync(User);

            if (user == null)
            {
                return Unauthorized(); // User not authenticated
            }

            var userPlantTasks = await _userTaskService.GetUserPlantTasks(user);

            return Ok(userPlantTasks);
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            return StatusCode(500, "Internal server error");
        }
    }

    //[Authorize]
    //[HttpPost("addTask")]
    //public async Task<IActionResult> AddTask([FromBody] UserTaskRequestDto taskDto)
    //{
    //    // Get the user ID from the claims
    //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

    //    // Ensure that the user ID is not null or empty
    //    if (string.IsNullOrEmpty(userId))
    //    {
    //        return BadRequest("Invalid user ID in claims.");
    //    }

    //    // Call the service to add the task
    //    var taskId = await _userPlantActivityService.AddTaskToUser(userId, taskDto);

    //    // Check the result from the service
    //    if (taskId == -1)
    //    {
    //        return BadRequest("User not found."); // You can customize error responses
    //    }

    //    if (taskId == -2)
    //    {
    //        return BadRequest("Plant not found."); // You can customize error responses
    //    }

    //    // Task added successfully
    //    return Ok(new { TaskId = taskId });
    //}


}
