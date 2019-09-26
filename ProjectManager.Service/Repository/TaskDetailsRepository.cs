namespace ProjectManager.Service.Repository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using ProjectManager.Service.Models;

    /// <summary>
    /// TaskDetailsRepository class
    /// </summary>
    public class TaskDetailsRepository : ITaskDetailsRepository
    {
        private readonly ProjectManagerDbContext _projectManagerDbContext;

        /// <summary>
        /// constructor for TaskDetailsRepository.
        /// </summary>
        /// <param name="projectManagerDbContext"></param>
        public TaskDetailsRepository(ProjectManagerDbContext projectManagerDbContext)
        {
            _projectManagerDbContext = projectManagerDbContext;
        }

        public async Task Delete(Models.TaskDetailModel entity)
        {
            _projectManagerDbContext.Tasks.Remove(entity);

            await _projectManagerDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Models.TaskDetailModel>> GetAllTasks()
        {
            return await _projectManagerDbContext.Tasks.Include(project => project.UserDetail)
                .Include(project => project.ProjectDetail).AsNoTracking<TaskDetailModel>().ToListAsync();
        }

        public async Task<Models.TaskDetailModel> Get(int id)
        {
            return await _projectManagerDbContext.Tasks.Include(project => project.UserDetail)
                .Include(project => project.ProjectDetail).AsNoTracking<TaskDetailModel>().FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<int> Insert(Models.TaskDetailModel entity)
        {
            entity.UserDetail = null;
            entity.ProjectDetail = null;
            _projectManagerDbContext.Tasks.Add(entity);

            return await _projectManagerDbContext.SaveChangesAsync();
        }

        public async Task Update(int id, Models.TaskDetailModel entity)
        {
            entity.ProjectDetail = null;
            entity.UserDetail = null;
            _projectManagerDbContext.Tasks.Update(entity);

            await _projectManagerDbContext.SaveChangesAsync();
        }
    }
}
