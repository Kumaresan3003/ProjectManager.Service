namespace ProjectManager.Service.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// UserDetailModel class.
    /// </summary>
    public class UserDetailModel
    {
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int EmployeeId { get; set; }

        public IList<ProjectDetailModel> Projects { get; set; }

        public IList<TaskDetailModel> TaskDetails { get; set; }
    }
}
