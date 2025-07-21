using LoginPage.Models;
using LoginPage.Services.Login;
using Microsoft.AspNetCore.Mvc;
using LoginPage.Services.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace LoginPage.ApiControllers;
[ApiController]
[Route("api/")]
[Authorize]
public class TasksApiController: ControllerBase
{
    private readonly ITasksService _tasksService;

    public TasksApiController(ITasksService tasksService)
    {
        _tasksService = tasksService;
    }



    [HttpGet]
    [Route("status")]
    public async Task<IActionResult> GetStatus()
    {
        var status = await _tasksService.GetStatus();
        if (status == null)
            return NotFound("No status found");
        return Ok(status);
    }


    /*[HttpGet]
    [Route("tasks")]
    public async Task<IActionResult> GetTasks()
    {
        var tasks = await _tasksService.GetTasks();
        if (tasks == null)
            return NotFound("No tasks found");
        return Ok(tasks);
    }*/
    
    // [HttpPost]
    // [Route("createTaskByAdmin")]
    // public async Task<IActionResult> CreateTaskByAdmin(Models.CreateTaskModel task)
    // {
    //     if (task == null)
    //         return BadRequest("Task cannot be null");
    //     
    //     
    //     var createdTask = await _tasksService.CreateTaskByAdmin(task);
    //     
    //     if (createdTask == null)
    //         return BadRequest("Failed to create task");
    //     
    //     return Ok(createdTask);
    // }
    
    [HttpPost]
    [Route("createTaskByUser")]
    public async Task<IActionResult> CreateTaskByUser(Models.CreateTaskModel task)
    {
        if (task == null)
            return BadRequest("Task cannot be null");
        
        
        var createdTask = await _tasksService.CreateTaskByUser(task);
        
        if (createdTask == null)
            return BadRequest("Failed to create task");
        
        return Ok(createdTask);
    }
    
    [HttpGet("tasks/{id}")]
    public IActionResult GetTaskForEdit(int id)
    {
        var task = _tasksService.GetTasksById(id);

        if (task == null)
            return NotFound();

        return Ok(task);
    }
    [HttpPut("updateTask/{taskId}")]
    public async Task<IActionResult> UpdateTask(int taskId, [FromBody] Models.CreateTaskModel task)
    {
        if (task == null)
            return BadRequest("Task cannot be null");

        var updatedTask = await _tasksService.UpdateTask(taskId, task);
        
        if (updatedTask == null)
            return NotFound("Task not found or update failed");

        return Ok(updatedTask);
    }

    [HttpDelete("deleteTask/{taskId}")]
    public async Task<IActionResult> DeleteTask(int taskId)
    {
        var task = await _tasksService.GetTasksById(taskId);
        if (task == null)
            return NotFound("Task not found");
        var result = await _tasksService.DeleteTask(taskId);
        if (!result)
            return BadRequest("Failed to delete task");
        
        return Ok("Task deleted successfully");
    }
   
}