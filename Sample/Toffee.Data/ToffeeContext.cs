using Igtampe.ChopoImageHandling;
using Igtampe.DBContexts;
using Igtampe.Notifier;
using Igtampe.Toffee.Common;
using Microsoft.EntityFrameworkCore;

namespace Igtampe.Toffee.Data {

    /// <summary>Main context for all of Toffee</summary>
    public class ToffeeContext : PostgresContext, INotificationContext<User>, IImageContext {

        private static readonly Category NONE_CATEGORY = new() {
            Name = "None", Description = "Tasks with no categories",
            Color = "", Creator = null, Icon = null, ID = Guid.Empty
        };

        /// <summary>A Pairing of Category and Count for the list of counts by category</summary>
        public struct TaskByCategoryCountItem {

            /// <summary>Category</summary>
            public Category Category { get; }

            /// <summary>Count of tasks in given category</summary>
            public int Count { get; }

            /// <summary>Creates a TaskByCategoryCount Item</summary>
            /// <param name="Category"></param>
            /// <param name="Count"></param>
            public TaskByCategoryCountItem(Category Category, int Count) {
                this.Category = Category;
                this.Count = Count;
            }
        }

        /// <summary>A Pairing of Status and Count for the list of counts by status</summary>
        public struct TaskByStatusCountItem {
            /// <summary>Category</summary>
            public Common.TaskStatus Status { get; }

            /// <summary>Count of tasks in given category</summary>
            public int Count { get; }

            /// <summary>Creates a TaskByCategoryCount Item</summary>
            /// <param name="Status"></param>
            /// <param name="Count"></param>
            public TaskByStatusCountItem(Common.TaskStatus Status, int Count) {
                this.Status = Status;
                this.Count = Count;
            }
        }

        /// <summary>Struct with count of all tasks</summary>
        public struct TaskCounts {

            /// <summary>Total count of all tasks</summary>
            public int TotalCount { get; }

            /// <summary>Breakdown of task counts by category</summary>
            public List<TaskByCategoryCountItem> ByCategory { get; }

            /// <summary>Breakdown of task status by status</summary>
            public List<TaskByStatusCountItem> ByStatus { get; }

            /// <summary>Creates a Task Count Summary</summary>
            /// <param name="Total"></param>
            /// <param name="ByCat"></param>
            /// <param name="ByStat"></param>
            public TaskCounts(int Total, List<TaskByCategoryCountItem> ByCat, List<TaskByStatusCountItem> ByStat) {
                TotalCount = Total;
                ByCategory = ByCat;
                ByStatus = ByStat;
            }
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        /// <summary>Set of all users</summary>
        public DbSet<User> User { get; set; }

        /// <summary>Set of all images</summary>
        public DbSet<Image> Image { get; set; }

        /// <summary>Set of all notifications</summary>
        public DbSet<Notification> Notification { get; set; }

        /// <summary>Set of all tasks</summary>
        public DbSet<Common.Task> Task { get; set; }

        /// <summary>Set of all Categories</summary>
        public DbSet<Category> Category { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        /// <summary>Applies all autoincludes on Users</summary>
        /// <param name="Set"></param>
        /// <returns></returns>
        public IQueryable<User> ApplyAutoIncludes(IQueryable<User> Set) => Set.Include(A=>A.ProfilePicture);

        /// <summary>Applies all AutoIncludes on Tasks</summary>
        /// <param name="Set"></param>
        /// <returns></returns>
        public IQueryable<Common.Task> ApplyAutoIncludes(IQueryable<Common.Task> Set)
            => Set
                .Include(A => A.Assigner).ThenInclude(A => A!.ProfilePicture)
                .Include(A => A.Assignee).ThenInclude(A => A!.ProfilePicture);

        /// <summary>Applies all AutoIncludes on Categories</summary>
        /// <param name="Set"></param>
        /// <returns></returns>
        public IQueryable<Category> ApplyAutoIncludes(IQueryable<Category> Set)
            => Set
                .Include(A => A.Icon)
                .Include(A => A.Creator).ThenInclude(A => A!.ProfilePicture);

        /// <summary>Get the categories owned by a user</summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public async Task<List<Category>> GetUserCategories(string Username) 
            => await ApplyAutoIncludes(Category).Where(A => A.Creator != null && A.Creator.Username == Username).ToListAsync();
        
        /// <summary>Gets a list of all categories</summary>
        /// <returns></returns>
        public async Task<List<Category>> GetAllCategories()
            => await ApplyAutoIncludes(Category).ToListAsync();

        /// <summary>Get the tasks assigned to a user</summary>
        /// <param name="Username"></param>
        /// <param name="Order"></param>
        /// <param name="Query"></param>
        /// <param name="Status"></param>
        /// <param name="CategoryID"></param>
        /// <param name="Skip"></param>
        /// <param name="Take"></param>
        /// <returns></returns>
        public async Task<List<Common.Task>> GetTasksAssignedToUser(string Username, TaskOrder Order, string Query, Common.TaskStatus? Status, Guid? CategoryID, int? Skip, int? Take)
            => await TaskCollectionToList(AllTaskFilters(AssignedToUser(ApplyAutoIncludes(Task), Username), Order, Query, Status, CategoryID),Skip,Take);
        
        /// <summary>Gets the tasks assigned by a user</summary>
        /// <param name="Username"></param>
        /// <param name="Order"></param>
        /// <param name="Query"></param>
        /// <param name="Status"></param>
        /// <param name="CategoryID"></param>
        /// <param name="Skip"></param>
        /// <param name="Take"></param>
        /// <returns></returns>
        public async Task<List<Common.Task>> GetTasksAssignedByUser(string Username, TaskOrder Order, string Query, Common.TaskStatus? Status, Guid? CategoryID, int? Skip, int? Take)
            => await TaskCollectionToList(AllTaskFilters(AssignedByUser(ApplyAutoIncludes(Task), Username), Order, Query, Status, CategoryID),Skip,Take);

        /// <summary>Converts a task collection to a list</summary>
        /// <param name="Tasks"></param>
        /// <param name="Skip"></param>
        /// <param name="Take"></param>
        /// <returns></returns>
        public async static Task<List<Common.Task>> TaskCollectionToList(IQueryable<Common.Task> Tasks, int? Skip, int? Take)
            => await Tasks
                .Include(A => A.Assignee)
                .Include(A => A.Assigner)
                .Include(A => A.Category)
                .Skip(Skip ?? 0).Take(Take ?? 20)
                .ToListAsync();

        /// <summary>Gets Task status and category statistics for the whole system</summary>
        /// <returns></returns>
        public async Task<TaskCounts> GetAllTaskCount() => await GetTaskCounts(Task);

        /// <summary>Gets Task Status and Category statistics for tasks assigned to a user</summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public async Task<TaskCounts> GetAssigneeTaskCount(string Username) => await GetTaskCounts(Task.Where(A => A.Assignee != null && A.Assignee.Username == Username));

        /// <summary>Gets Task Status and Category Statistics for Tasks asisgned by a user</summary>
        /// <param name="Username"></param>
        /// <returns></returns>
        public async Task<TaskCounts> GetAssignerTaskCount(string Username) => await GetTaskCounts(Task.Where(A => A.Assigner != null && A.Assigner.Username == Username));

        private static async Task<TaskCounts> GetTaskCounts(IQueryable<Common.Task> Set) {
            var TotalCount = await Set.CountAsync();
            var ByCat = await Set.GroupBy(A => A.Category).Select(A => new TaskByCategoryCountItem(A.Key ?? NONE_CATEGORY, A.Count())).ToListAsync(); //I really hope this works
            var ByStat = await Set.GroupBy(A => A.Status).Select(A => new TaskByStatusCountItem(A.Key, A.Count())).ToListAsync();
            return new(TotalCount,ByCat,ByStat);
        }

        /// <summary>Runs all filters on a collection of tasks</summary>
        /// <param name="Tasks"></param>
        /// <param name="Order"></param>
        /// <param name="Query"></param>
        /// <param name="Status"></param>
        /// <param name="CategoryID"></param>
        /// <returns></returns>
        private static IQueryable<Common.Task> AllTaskFilters(IQueryable<Common.Task> Tasks, TaskOrder Order, string Query, Common.TaskStatus? Status, Guid? CategoryID)
            => OrderTasks(SearchTasks(FilterByStatus(FilterByCategory(Tasks,CategoryID), Status),Query),Order);

        /// <summary>Filters a collection of tasks by the username that it was assigend to</summary>
        /// <param name="Tasks"></param>
        /// <param name="Username"></param>
        /// <returns></returns>
        private static IQueryable<Common.Task> AssignedToUser(IQueryable<Common.Task> Tasks, string Username)
            => Tasks.Where(A => A.Assignee != null && A.Assignee.Username == Username);

        /// <summary>Filters a collection of tasks by the Username that it was assigned by</summary>
        /// <param name="Tasks"></param>
        /// <param name="Username"></param>
        /// <returns></returns>
        private static IQueryable<Common.Task> AssignedByUser(IQueryable<Common.Task> Tasks, string Username)
            => Tasks.Where(A => A.Assigner != null && A.Assigner.Username == Username);

        /// <summary>Order a task collection by a specified order</summary>
        /// <param name="Tasks"></param>
        /// <param name="Order"></param>
        /// <returns></returns>
        private static IQueryable<Common.Task> OrderTasks(IQueryable<Common.Task> Tasks, TaskOrder Order) {
            return Order switch {
                TaskOrder.DUE_DATE => Tasks.OrderBy(A => A.DueDate),
                TaskOrder.ASSIGNED_DATE => Tasks.OrderBy(A => A.DateAssigned),
                TaskOrder.PRIORITY => Tasks.OrderByDescending(A => A.Priority),
                TaskOrder.NAME => Tasks.OrderBy(A => A.Name),
                _ => Tasks,
            };
        }

        /// <summary>Filters a collection of tasks by a search query</summary>
        /// <param name="Tasks"></param>
        /// <param name="Query"></param>
        /// <returns></returns>
        private static IQueryable<Common.Task> SearchTasks(IQueryable<Common.Task> Tasks, string Query) 
            => string.IsNullOrWhiteSpace(Query)? Tasks : Tasks.Where(A=>
                A.Name.ToLower().Contains(Query) || 
                A.Description.ToLower().Contains(Query) || 
                A.StatusMessage.ToLower().Contains(Query)
            );

        /// <summary>Filter a collection of tasks by a status</summary>
        /// <param name="Tasks"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        private static IQueryable<Common.Task> FilterByStatus(IQueryable<Common.Task> Tasks, Common.TaskStatus? Status)
            => Status is null ? Tasks : Tasks.Where(A=>A.Status == Status);

        private static IQueryable<Common.Task> FilterByCategory(IQueryable<Common.Task> Tasks, Guid? CategoryID)
            => CategoryID is null ? Tasks : Tasks.Where(A=>A.Category!=null && A.Category.ID==CategoryID);
        }
}