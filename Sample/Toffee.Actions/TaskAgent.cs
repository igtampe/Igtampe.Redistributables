using Igtampe.Actions;
using Igtampe.ChopoSessionManager;
using Igtampe.Toffee.Common;
using Igtampe.Toffee.Common.Exceptions;
using Igtampe.Toffee.Data;
using Microsoft.EntityFrameworkCore;
using static Igtampe.Toffee.Common.TaskStateTransitionChecker;
using static Igtampe.Toffee.Data.ToffeeContext;

namespace Igtampe.Toffee.Actions {

    /// <summary>Agent to act upon all Tasks</summary>
    public class TaskAgent : SessionedActionAgent<ToffeeContext> {

        private UserAgent UserAgent { get; } //YOU CAN CHAIN AGENTS WOW
        private CategoryAgent CategoryAgent { get; }
        private NotificationAgent<ToffeeContext,Notification,User> NotifAgent { get; }

        /// <summary>Creates a Task Agent</summary>
        /// <param name="Context"></param>
        /// <param name="Manager"></param>
        public TaskAgent(ToffeeContext Context, ISessionManager Manager) : base(Context, Manager) {
            UserAgent = new(Context, Manager);
            CategoryAgent = new(Context, Manager);
            NotifAgent = new(Context, Manager);
        }
        
        /// <summary>Gets tasks assigned to a signed in user</summary>
        /// <param name="SessionID"></param>
        /// <param name="Order"></param>
        /// <param name="Query"></param>
        /// <param name="Status"></param>
        /// <param name="CategoryID"></param>
        /// <param name="Skip"></param>
        /// <param name="Take"></param>
        /// <returns></returns>
        public async Task<List<Common.Task>> GetTasksAssignedTo(Guid? SessionID, TaskOrder Order, string Query, Common.TaskStatus? Status, Guid? CategoryID, int? Skip, int? Take)
            => await Context.GetTasksAssignedToUser((await GetSession(SessionID)).Username, Order, Query, Status, CategoryID, Skip, Take);

        /// <summary>Gets tasks assigned by a signed in user</summary>
        /// <param name="SessionID"></param>
        /// <param name="Order"></param>
        /// <param name="Query"></param>
        /// <param name="Status"></param>
        /// <param name="CategoryID"></param>
        /// <param name="Skip"></param>
        /// <param name="Take"></param>
        /// <returns></returns>
        public async Task<List<Common.Task>> GetTasksAssignedBy(Guid? SessionID, TaskOrder Order, string Query, Common.TaskStatus? Status, Guid? CategoryID, int? Skip, int? Take)
            => await Context.GetTasksAssignedByUser((await GetSession(SessionID)).Username, Order, Query, Status, CategoryID, Skip, Take);

        /// <summary>Gets count data of Tasks assigned to a signed in user</summary>
        /// <param name="SessionID"></param>
        /// <returns></returns>
        public async Task<TaskCounts> GetTasksAssignedToCount(Guid? SessionID)
            => await Context.GetAssigneeTaskCount((await GetSession(SessionID)).Username);
        
        /// <summary>Gets count data of tasks assigned by a signed in user</summary>
        /// <param name="SessionID"></param>
        /// <returns></returns>
        public async Task<TaskCounts> GetTasksAssignedByCount(Guid? SessionID)
            => await Context.GetAssignerTaskCount((await GetSession(SessionID)).Username);

        /// <summary>Gets count of tasks in the entire context</summary>
        /// <returns></returns>
        public async Task<TaskCounts> GetTasksCount()
            => await Context.GetAllTaskCount();

        /// <summary>Assigns a new task</summary>
        /// <param name="SessionID">ID of the Session execuing this request</param>
        /// <param name="Username">Username to assign to</param>
        /// <param name="Name">Name of the task</param>
        /// <param name="Description">Description of the task</param>
        /// <param name="DueDate">Date this item is due</param>
        /// <param name="Priority">Priority of this item</param>
        /// <param name="CategoryID">Optional Category ID</param>
        /// <param name="Notify">Whether or not to notify the assignee</param>
        /// <returns></returns>
        /// <exception cref="TaskUnassignableException">Thrown if Task is unassignable by the assigner due to the assignee having a higher rank</exception>
        public async Task<Common.Task> AssignTask(Guid? SessionID, string Username, 
            string Name, string Description, DateTime DueDate, TaskPriority Priority, Guid? CategoryID, bool Notify) {

            //Get the Session and relevant users
            var S = await GetSession(SessionID);
            var Assigner = await UserAgent.GetUser(S.Username);
            var Assignee = await UserAgent.GetUser(Username);

            if (Assigner.Rank < Assignee.Rank) { throw new TaskUnassignableException(Assigner.Rank,Assignee.Rank); }

            Common.Task T = new() {
                Assignee = Assignee, Assigner = Assigner, DateAssigned = DateTime.UtcNow,
                Status = Common.TaskStatus.ASSIGNED, StatusMessage = "",
                DueDate = DueDate, Priority = Priority, 
                Name = Name, Description = Description,
            };

            if (CategoryID != null) { T.Category = await CategoryAgent.GetCategory(CategoryID.Value); }

            Context.Task.Add(T);
            await Context.SaveChangesAsync();

            if (Notify) {
                await NotifAgent.SendNotification(new() {
                    Owner = Assignee,
                    Text = $"You have been assigned \"{T.Name}\" by {Assigner.Name}",
                    Task = T
                });
            }

            return T;
        }

        /// <summary>Reassigns a dropped task</summary>
        /// <param name="SessionID">ID of the Session Executing this request</param>
        /// <param name="Username">Username to reassign to</param>
        /// <param name="TaskID">ID of the task to reassign</param>
        /// <param name="Notify">Whether or not to notify the new assignee</param>
        /// <returns></returns>
        /// <exception cref="NotTaskAssignerException">Thrown if Session owner is not Task Assigner</exception>
        /// <exception cref="TaskStateTransitionException">Thrown if the Task is not currently dropped</exception>
        /// <exception cref="TaskUnassignableException">Thrown if Task is unassignable by the assigner due to the assignee having a higher rank</exception>
        public async Task<Common.Task> ReassignTask(Guid? SessionID, string Username, Guid TaskID, bool Notify) {

            //Get the task and check that its currently Dropped
            var T = await GetTask(TaskID);
            if (T.Status != Common.TaskStatus.DROPPED && T.Status !=Common.TaskStatus.REJECTED) { 
                throw new TaskStateTransitionException(TaskID, T.Status, 
                    Common.TaskStatus.REASSIGNED, TaskStateTransitionChecker.Executor.Assigner); }

            //Get Session and relevant users
            var S = await GetSession(SessionID);
            var Assigner = await UserAgent.GetUser(S.Username);
            var Assignee = await UserAgent.GetUser(Username);

            if (S.Username != T.Assigner?.Username) { throw new NotTaskAssignerException(TaskID); }
            if (Assigner.Rank < Assignee.Rank) { throw new TaskUnassignableException(Assigner.Rank, Assignee.Rank); }

            T.Assignee= Assignee;
            T.DateAssigned = DateTime.UtcNow;
            T.Status = Common.TaskStatus.REASSIGNED;
            T.StatusMessage = "";

            await SaveTask(T);

            if (Notify) {
                await NotifAgent.SendNotification(new() {
                    Owner = Assignee,
                    Text = $"You have been assigned \"{T.Name}\" by {Assigner.Name}",
                    Task=T
                }); 
            }

            return T;

        }

        /// <summary>Updates a Task's Status</summary>
        /// <param name="SessionID"></param>
        /// <param name="TaskID"></param>
        /// <param name="Status"></param>
        /// <param name="StatusMessage"></param>
        /// <param name="Notify"></param>
        /// <returns></returns>
        public async Task<Common.Task> UpdateTaskStatus(Guid? SessionID, Guid TaskID, Common.TaskStatus Status, string StatusMessage, bool Notify) {
            //Get the task
            var T = await GetTask(TaskID);

            //Get Session
            var S = await GetSession(SessionID);

            //Determine who is doing this
            Executor TaskExecutor = T.Assigner is not null && T.Assigner.Username == S.Username
                ? Executor.Assigner
                : T.Assignee is not null && T.Assignee.Username == S.Username
                    ? Executor.Assigner
                    : await UserAgent.UserIsAdmin(S.Username) 
                        ? Executor.Admin 
                        : throw new TaskNotFoundException(TaskID);

            //I really love Ternary operators. Just format them right and they can be readable

            //Check the transition
            if (!CheckStateTransition(T.Status,Status,TaskExecutor)) { throw new TaskStateTransitionException(TaskID, T.Status, Status, TaskExecutor); }

            //Actually do this
            T.Status = Status;
            T.StatusMessage = StatusMessage;
            await SaveTask(T);

            if (Notify) {
                await NotifAgent.SendNotification(new() {
                    Owner = T.Assigner,
                    Text = $"Task \"{T.Name}\" is now {nameof(T.Status).ToLower().Replace("_", "")}",
                    Task = T
                });
            }

            return T;
        }
        //Updates Task Category, Name, 

        /// <summary>Updates a Task's details</summary>
        /// <param name="SessionID"></param>
        /// <param name="TaskID"></param>
        /// <param name="Name"></param>
        /// <param name="Description"></param>
        /// <param name="DueDate"></param>
        /// <param name="Priority"></param>
        /// <param name="CategoryID"></param>
        /// <param name="Notify"></param>
        /// <returns></returns>
        /// <exception cref="NotTaskAssignerException">Thrown if session owner is not the Task assigner</exception>
        public async Task<Common.Task> UpdateTask(Guid? SessionID, Guid TaskID,
            string Name, string Description, DateTime DueDate, TaskPriority Priority, Guid? CategoryID, bool Notify) {

            //Get the task
            var T = await GetTask(TaskID);

            //Get Session
            var S = await GetSession(SessionID);

            //Assert the Session Owner is the Assigner of this task
            if (T.Assigner is null || T.Assigner.Username != S.Username) { throw new NotTaskAssignerException(TaskID); }

            T.Name = Name;
            T.Description = Description;
            T.DueDate = DueDate;
            T.Priority = Priority;

            //Deal with updating the category
            if (CategoryID is not null) { T.Category = await CategoryAgent.GetCategory(CategoryID.Value); }  //If the categoryID is not null, then we must update the category
            else if (T.Category is not null) { T.Category = null; } //Else, if the category IS null, and the Category of the task ISN'T null, then we must match
            //I don't think EF will update that but sabes its the thought that counts.

            await SaveTask(T);
            
            if (Notify) {
                await NotifAgent.SendNotification(new() {
                    Owner = T.Assignee,
                    Text = $"Task \"{T.Name}\" has been updated",
                    Task = T
                });
            }

            return T;

        }
        //Updates Task Category, Name, Description, Priority, and DueDate. Only accessible to Assigner

        /// <summary>Saves a Task</summary>
        /// <param name="T"></param>
        /// <returns></returns>
        private async Task<Common.Task> SaveTask(Common.Task T) {
            Context.Task.Update(T);
            await Context.SaveChangesAsync();
            return T;
        }

        /// <summary>Get a specific task</summary>
        /// <param name="TaskID"></param>
        /// <returns></returns>
        /// <exception cref="TaskNotFoundException"></exception>
        public async Task<Common.Task> GetTask(Guid TaskID)
            => await Context.ApplyAutoIncludes(Context.Task).FirstOrDefaultAsync(A => A.ID == TaskID) 
            ?? throw new TaskNotFoundException(TaskID);

    }
}
