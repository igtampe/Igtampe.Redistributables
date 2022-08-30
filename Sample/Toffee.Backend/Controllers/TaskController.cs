using Igtampe.ChopoSessionManager;
using Igtampe.Toffee.Actions;
using Igtampe.Toffee.Backend.Requests.Task;
using Igtampe.Toffee.Common;
using Igtampe.Toffee.Data;
using Microsoft.AspNetCore.Mvc;

namespace Igtampe.Toffee.Backend.Controllers {

    [Route("API/Tasks")]
    [ApiController]
    public class TaskController : ControllerBase {

        private readonly TaskAgent Agent;

        public TaskController(ToffeeContext Context)
            => Agent = new(Context, SessionManager.Manager);

        [HttpGet("To")]
        public async Task<IActionResult> GetTasksAssignedTo([FromHeader] Guid? SessionID, [FromQuery] TaskOrder? Order, [FromQuery] string? Query,
            [FromQuery] Common.TaskStatus? Status, [FromQuery] Guid? CategoryID, [FromQuery] int? Skip, [FromQuery] int? Take)
            => Ok(await Agent.GetTasksAssignedTo(SessionID, Order ?? TaskOrder.DUE_DATE, Query ?? "", Status, CategoryID, Skip, Take));
        
        [HttpGet("By")]
        public async Task<IActionResult> GetTasksAssignedBy([FromHeader] Guid? SessionID, [FromQuery] TaskOrder? Order, [FromQuery] string? Query,
            [FromQuery] Common.TaskStatus? Status, [FromQuery] Guid? CategoryID, [FromQuery] int? Skip, [FromQuery] int? Take)
            => Ok(await Agent.GetTasksAssignedBy(SessionID, Order ?? TaskOrder.DUE_DATE, Query ?? "", Status, CategoryID, Skip, Take));

        [HttpGet("To/Count")]
        public async Task<IActionResult> GetTasksAssignedToCount([FromHeader] Guid? SessionID) => Ok(await Agent.GetTasksAssignedToCount(SessionID));
        
        [HttpGet("By/Count")]
        public async Task<IActionResult> GetTasksAssignedByCount([FromHeader] Guid? SessionID) => Ok(await Agent.GetTasksAssignedByCount(SessionID));

        [HttpGet("Count")]
        public async Task<IActionResult> GetTasksCount() => Ok(await Agent.GetTasksCount());

        [HttpGet("{ID}")]
        public async Task<IActionResult> GetTask([FromRoute] Guid ID) => Ok(await Agent.GetTask(ID));

        [HttpPost]
        public async Task<IActionResult> AssignTask([FromHeader] Guid? SessionID, [FromBody] TaskAssignerRequest Request)
            => Ok(await Agent.AssignTask(SessionID, Request.Username, Request.Name, Request.Description, Request.DueDate, Request.Priority, 
                Request.CategoryID, Request.Notify));

        [HttpPut("{ID}/Reassign")]
        public async Task<IActionResult> ReassignTask([FromHeader] Guid? SessionID, [FromRoute] Guid ID, [FromBody] TaskAssignerRequest Request)
            => Ok(await Agent.ReassignTask(SessionID, Request.Username, ID, Request.Notify));

        [HttpPut("{ID}/Assigner")]
        public async Task<IActionResult> UpdateTask([FromHeader] Guid? SessionID, [FromRoute] Guid ID, [FromBody] TaskAssignerRequest Request)
            => Ok(await Agent.UpdateTask(SessionID, ID, Request.Name, Request.Description, Request.DueDate, Request.Priority,
                Request.CategoryID, Request.Notify));

        [HttpPut("{ID}/Assignee")]
        public async Task<IActionResult> UpdateTaskStatus([FromHeader] Guid? SessionID, [FromRoute] Guid ID, [FromBody] TaskAssigneeRequest Request)
            => Ok(await Agent.UpdateTaskStatus(SessionID, ID, Request.Status, Request.StatusMessage, Request.Notify));

    }
}
