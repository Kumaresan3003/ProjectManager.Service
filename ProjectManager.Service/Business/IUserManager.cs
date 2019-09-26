namespace ProjectManager.Service.Business
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ProjectManager.Service.Models;

    /// <summary>
    /// Interface for UserManager.
    /// </summary>
    public interface IUserManager
    {
        Task<IEnumerable<UserDetailModel>> GetAllUsers();

        Task<UserDetailModel> GetUserDetail(int id);

        Task<int> AddUserDetails(UserDetailModel userDetailModel);

        Task UpdateUserDetails(int id, UserDetailModel userDetailModel);

        bool IsUserValid(UserDetailModel userDetailModel);

        Task RemoveUser(UserDetailModel userDetailModel);
    }
}
