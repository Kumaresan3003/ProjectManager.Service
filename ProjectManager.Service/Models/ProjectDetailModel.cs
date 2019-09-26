namespace ProjectManager.Service.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// ProjectDetailModel class.
    /// </summary>
    public class ProjectDetailModel
    {
        public int ProjectId { get; set; }

        public string ProjectName { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int Priority { get; set; }

        public int UserId { get; set; }


        public bool EndProject { get; set; }

        public List<TaskDetailModel> TaskDetails { get; set; }

        public UserDetailModel UserDetail { get; set; }
    }
}
