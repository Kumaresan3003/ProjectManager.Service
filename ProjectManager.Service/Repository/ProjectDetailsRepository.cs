namespace ProjectManager.Service.Repository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using ProjectManager.Service.Models;

    /// <summary>
    /// ProjectDetailsRepository class.
    /// </summary>
    public class ProjectDetailsRepository : IProjectDetailsRepository
    {
        private readonly ProjectManagerDbContext _projectManagerDbContext;

        /// <summary>
        /// Condtructor for ProjectDetailsRepository.
        /// </summary>
        /// <param name="projectManagerDbContext"></param>
        public ProjectDetailsRepository(ProjectManagerDbContext projectManagerDbContext)
        {
            _projectManagerDbContext = projectManagerDbContext;
        }
        public async Task Delete(ProjectDetailModel entity)
        {
            _projectManagerDbContext.Projects.Remove(entity);

            await _projectManagerDbContext.SaveChangesAsync();
        }

        public async Task<ProjectDetailModel> Get(int id)
        {
            return await _projectManagerDbContext.Projects.
                Include(project => project.TaskDetails).Include(project => project.UserDetail).FirstOrDefaultAsync(t => t.ProjectId == id);
        }

        public  async Task<IEnumerable<ProjectDetailModel>> GetAllProjects()
        {
            return await _projectManagerDbContext.Projects.Include(project => project.TaskDetails).Include(project => project.UserDetail)
                .AsNoTracking<ProjectDetailModel>().ToListAsync();
        }

        public async Task<int> Insert(ProjectDetailModel entity)
        {
            entity.UserDetail = null;
            _projectManagerDbContext.Projects.Add(entity);
            return await _projectManagerDbContext.SaveChangesAsync();
        }

        public async Task Update(int id, ProjectDetailModel entity)
        {
            _projectManagerDbContext.Projects.Update(entity);

            await _projectManagerDbContext.SaveChangesAsync();
        }
    }
}
