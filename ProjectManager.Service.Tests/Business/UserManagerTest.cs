namespace ProjectManager.Service.Tests.Business
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Moq;
    using ProjectManager.Service.Business;
    using ProjectManager.Service.Repository;
    using Xunit;

    public class UserManagerTest : IDisposable
    {
        public ILogger<UserManager> Logger { get; }

        /// <summary>
        /// Constructor for UserManagerTest
        /// </summary>
        public UserManagerTest()
        {
            ServiceProvider serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            ILoggerFactory factory = serviceProvider.GetService<ILoggerFactory>();
            Logger = factory.CreateLogger<UserManager>();
        }

        [Fact]
        public async Task VerifyUserInsertCalledOnce()
        {
            // Arrange
            var mockRepository = new Mock<IUserDetailsRepository>();
            var userManger = new UserManager(mockRepository.Object);
            var taskDetail = new Models.UserDetailModel();

            // Act
            await userManger.AddUserDetails(taskDetail);

            // Assert
            mockRepository.Verify(t => t.Insert(taskDetail), Times.Once);
        }

        [Fact]
        public async Task VerifyUserUpdateCalledOnce()
        {
            // Arrange
            var mockRepository = new Mock<IUserDetailsRepository>();
            var userManger = new UserManager(mockRepository.Object);
            var taskDetail = new Models.UserDetailModel();

            // Act
            await userManger.UpdateUserDetails(10, taskDetail);

            // Assert
            mockRepository.Verify(t => t.Update(10, taskDetail), Times.Once);
        }

        [Fact]
        public async Task VerifyUserGetAllUsersCalledOnce()
        {            // Arrange
            Mock<IUserDetailsRepository> mockRepository = new Mock<IUserDetailsRepository>();
            var userManger = new UserManager(mockRepository.Object);

            // Act
            await userManger.GetAllUsers();

            // Assert
            mockRepository.Verify(t => t.GetAllUsers(), Times.Once);
        }

        [Fact]
        public async Task VerifyUserGetCalledOnce()
        {
            // Arrange
            var mockRepository = new Mock<IUserDetailsRepository>();
            var userManger = new UserManager(mockRepository.Object);

            // Act
            await userManger.GetUserDetail(10);

            // Assert
            mockRepository.Verify(t => t.Get(10), Times.Once);
        }

        #region IDisposable Support
        private bool _disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;
            if (disposing)
            {
            }

            _disposedValue = true;
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
