namespace Igtampe.Toffee.Common {

    /// <summary>A Toffee To-Do Task</summary>
    public class Task : AutomaticallyGeneratableIdentifiable, Nameable, Describable {

        //An Automatically Generatable Identifiable takes care of making an ID field that's set to auto populate when sent to an EF
        //Database with a GUID. All we need to put here is any fields/methods for this object
        
        /// <summary>Name of this task</summary>
        public string Name { get; set; } = "";

        /// <summary>Description of this Task</summary>
        public string Description { get; set; } = "";
    
        /// <summary>Category of this task</summary>
        public Category? Category { get; set; }

        /// <summary>Toffee user who assigned this task</summary>
        public User? Assigner { get; set; }

        /// <summary>Toffee user to whom this task is assigned</summary>
        public User? Assignee { get; set; }

        /// <summary>Defines if this task is in a read-only state (this does not prevent Reassignment)</summary>
        public bool Readonly => Status is TaskStatus.REJECTED or TaskStatus.DROPPED or TaskStatus.COMPLETED or TaskStatus.CANCELLED;

        /// <summary>Status of this Task</summary>
        public TaskStatus Status { get; set; } = TaskStatus.ASSIGNED;

        /// <summary>Message from the assignee about the current status</summary>
        public string StatusMessage { get; set; } = "";

        /// <summary>Priority of this Task</summary>
        public TaskPriority Priority { get; set; } = TaskPriority.NONE;

        /// <summary>Date this task was assigned</summary>
        public DateTime DateAssigned { get; set; } = DateTime.UtcNow;

        /// <summary>Due Date of this Task</summary>
        public DateTime DueDate { get; set; } = DateTime.UtcNow;

        //Dates and times are difficult for Javascript so let's just deal with this here

        /// <summary>Days until this task is due</summary>
        public int DaysToDue => Convert.ToInt32((DueDate - DateTime.UtcNow).TotalDays);

        /// <summary>Whether or not Task is due this week</summary>
        public bool DueThisWeek => DatesAreInTheSameWeek(DateTime.UtcNow, DueDate);

        /// <summary>From DXCK on Stack Overflow: https://stackoverflow.com/questions/25795254/check-if-a-datetime-is-in-same-week-as-other-datetime</summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        private static bool DatesAreInTheSameWeek(DateTime date1, DateTime date2) {
            var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
            var d1 = date1.Date.AddDays(-1 * (int)cal.GetDayOfWeek(date1));
            var d2 = date2.Date.AddDays(-1 * (int)cal.GetDayOfWeek(date2));

            return d1 == d2;
        } 
        //If this was this difficult on C# who knows how difficult it would've been on vanilla Javascript without some library
    }
}
