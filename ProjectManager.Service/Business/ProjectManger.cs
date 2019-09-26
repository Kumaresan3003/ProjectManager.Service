namespace ProjectManager.Service.Business
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ProjectManager.Service.Models;
    using ProjectManager.Service.Repository;

    /// <summary>
    /// Project Manager Class.
    /// </summary>
    public class ProjectManger : IProjectManager
    {
        private readonly IProjectDetailsRepository _projectDetailsRepository;

        /// <summary>
        /// Constructor for ProjectManger.
        /// </summary>
        /// <param name="projectDetailsRepository"></param>
        public ProjectManger(IProjectDetailsRepository projectDetailsRepository)
        {
            _projectDetailsRepository = projectDetailsRepository;
        }


        public async Task<int> AddProjectDetails(ProjectDetailModel project)
        {
          return await _projectDetailsRepository.Insert(project);
        }

        public async Task<IEnumerable<ProjectDetailModel>> GetAllProjects()
        {
            return await _projectDetailsRepository.GetAllProjects();
        }

        public async Task<ProjectDetailModel> GetProjectDetail(int id)
        {
            return await _projectDetailsRepository.Get(id);
        }

        public bool IsProjectValid(ProjectDetailModel project)
        {
            var isValid = !project.TaskDetails.Any(taskDetail => taskDetail.EndTask);
            return isValid;
        }

        public async Task RemoveProject(ProjectDetailModel project)
        {
            await _projectDetailsRepository.Delete(project);
        }

        public async Task UpdateProjectDetails(int id, ProjectDetailModel project)
        {
            await _projectDetailsRepository.Update(id, project);
        }
    }
}
