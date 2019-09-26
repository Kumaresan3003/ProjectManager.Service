namespace ProjectManager.Service.Business
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ProjectManager.Service.Models;
    using ProjectManager.Service.Repository;

    /// <summary>
    /// Class for UserManager.
    /// </summary>
    public class UserManager : IUserManager
    {
        private readonly IUserDetailsRepository _userDetailsRepository;

        public UserManager(IUserDetailsRepository userDetailsRepository)
        {
            _userDetailsRepository = userDetailsRepository;
        }

        public async Task<int> AddUserDetails(UserDetailModel userDetailModel)
        {
            return await _userDetailsRepository.Insert(userDetailModel);
        }

        public async Task<IEnumerable<UserDetailModel>> GetAllUsers()
        {
            return await _userDetailsRepository.GetAllUsers();
        }

        public async Task<UserDetailModel> GetUserDetail(int id)
        {
            return await _userDetailsRepository.Get(id);
        }

        public bool IsUserValid(UserDetailModel userDetailModel)
        {
            var isValid = !((userDetailModel.Projects != null && userDetailModel.Projects.Count > 0)
                || (userDetailModel.TaskDetails != null && userDetailModel.TaskDetails.Any(taskDetail => taskDetail.EndTask)));

            return isValid;
        }

        public async Task UpdateUserDetails(int id, UserDetailModel userDetailModel)
        {
            await _userDetailsRepository.Update(id, userDetailModel);
        }

        public async Task RemoveUser(UserDetailModel userDetailModel)
        {
            await _userDetailsRepository.Delete(userDetailModel);
        }
    }
}
