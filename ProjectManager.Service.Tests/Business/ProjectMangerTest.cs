namespace ProjectManger.Service.Tests.Business
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Moq;
    using ProjectManager.Service.Business;
    using ProjectManager.Service.Models;
    using ProjectManager.Service.Repository;
    using Xunit;

    public class ProjectMangerTest: IDisposable
    {
        public ILogger<ProjectManger> Logger { get; }

        /// <summary>
        /// Constructor for ProjectMangerTest
        /// </summary>
        public ProjectMangerTest()
        {
            ServiceProvider serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            ILoggerFactory factory = serviceProvider.GetService<ILoggerFactory>();
            Logger = factory.CreateLogger<ProjectManger>();
        }

        [Fact]
        public async Task VerifyProjectInsertCalledOnce()
        {
            // Arrange
            var mockRepository = new Mock<IProjectDetailsRepository>();
            var userManger = new ProjectManger(mockRepository.Object);
            var projectDetail = new ProjectDetailModel();

            // Act
            await userManger.AddProjectDetails(projectDetail);

            // Assert
            mockRepository.Verify(t => t.Insert(projectDetail), Times.Once);
        }

        [Fact]
        public async Task VerifyProjectUpdateCalledOnce()
        {
            // Arrange
            var mockRepository = new Mock<IProjectDetailsRepository>();
            var userManger = new ProjectManger(mockRepository.Object);
            var projectDetail = new ProjectDetailModel();

            // Act
            await userManger.UpdateProjectDetails(10, projectDetail);

            // Assert
            mockRepository.Verify(t => t.Update(10, projectDetail), Times.Once);
        }

        [Fact]
        public async Task VerifyUserGetAllProjectsCalledOnce()
        {            // Arrange
            Mock<IProjectDetailsRepository> mockRepository = new Mock<IProjectDetailsRepository>();
            var projectManger = new ProjectManger(mockRepository.Object);

            // Act
            await projectManger.GetAllProjects();

            // Assert
            mockRepository.Verify(t => t.GetAllProjects(), Times.Once);
        }

        [Fact]
        public async Task VerifyProjectGetCalledOnce()
        {
            // Arrange
            var mockRepository = new Mock<IProjectDetailsRepository>();
            var projectManger = new ProjectManger(mockRepository.Object);

            // Act
            await projectManger.GetProjectDetail(10);

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
