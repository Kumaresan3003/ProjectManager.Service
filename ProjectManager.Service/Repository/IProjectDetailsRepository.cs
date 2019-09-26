namespace ProjectManager.Service.Repository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ProjectManager.Service.Models;

    /// <summary>
    /// Interface for ProjectDetailsRepository.
    /// </summary>
    public interface IProjectDetailsRepository
    {

        Task<IEnumerable<ProjectDetailModel>> GetAllProjects();

        Task<ProjectDetailModel> Get(int id);

        Task<int> Insert(ProjectDetailModel entity);

        Task Update(int id, ProjectDetailModel entity);

        Task Delete(ProjectDetailModel entity);
    }
}
