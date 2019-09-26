namespace ProjectManager.Service.Repository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using ProjectManager.Service.Models;

    public class UserDetailsRepository : IUserDetailsRepository
    {
        private readonly ProjectManagerDbContext _projectManagerDbContext;

        public UserDetailsRepository(ProjectManagerDbContext projectManagerDbContext)
        {
            _projectManagerDbContext = projectManagerDbContext;
        }
        public async Task Delete(Models.UserDetailModel entity)
        {
            _projectManagerDbContext.Users.Remove(entity);

            await _projectManagerDbContext.SaveChangesAsync();
        }

        public async Task<Models.UserDetailModel> Get(int id)
        {
            return await _projectManagerDbContext.Users.Include(userObject => userObject.Projects).Include(userObject => userObject.TaskDetails)
                .FirstOrDefaultAsync(t => t.UserId == id);
        }

        public async Task<IEnumerable<UserDetailModel>> GetAllUsers()
        {
            return await _projectManagerDbContext.Users.AsNoTracking<UserDetailModel>().ToListAsync();
        }

        public async Task<int> Insert(UserDetailModel entity)
        {
            _projectManagerDbContext.Users.Add(entity);
            return await _projectManagerDbContext.SaveChangesAsync();
        }

        public async Task Update(int id, UserDetailModel entity)
        {
            _projectManagerDbContext.Users.Update(entity);
            await _projectManagerDbContext.SaveChangesAsync();
        }
    }
}
