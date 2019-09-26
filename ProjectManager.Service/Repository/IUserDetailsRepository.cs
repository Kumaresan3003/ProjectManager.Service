namespace ProjectManager.Service.Repository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ProjectManager.Service.Models;

    public interface IUserDetailsRepository
    {
        Task<IEnumerable<UserDetailModel>> GetAllUsers();

        Task<UserDetailModel> Get(int id);

        Task<int> Insert(UserDetailModel entity);

        Task Update(int id, UserDetailModel entity);

        Task Delete(UserDetailModel entity);
    }
}
