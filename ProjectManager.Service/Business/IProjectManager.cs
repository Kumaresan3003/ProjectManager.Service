namespace ProjectManager.Service.Business
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ProjectManager.Service.Models;

    /// <summary>
    /// Interface for ProjectManager.
    /// </summary>
    public interface IProjectManager
    {
        Task<IEnumerable<ProjectDetailModel>> GetAllProjects();

        Task<ProjectDetailModel> GetProjectDetail(int id);

        Task<int> AddProjectDetails(ProjectDetailModel project);

        Task UpdateProjectDetails(int id, ProjectDetailModel project);

        Task RemoveProject(ProjectDetailModel project);

        bool IsProjectValid(ProjectDetailModel project);
    }
}
