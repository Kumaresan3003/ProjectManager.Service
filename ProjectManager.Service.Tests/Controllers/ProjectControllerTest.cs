namespace ProjectManager.Service.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Service.Business;
    using Service.Controllers;
    using Xunit;

    public class ProjectControllerTest : IDisposable
    {
        public ILogger<ProjectController> Logger { get; }

        public ProjectControllerTest()
        {
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            Logger = factory.CreateLogger<ProjectController>();
        }

        [Fact]
        public async Task VerifyGetAllProjects_Returns_TwoprojectDetails()
        {
            // Arrange
            var mockManageProject = new Mock<IProjectManager>();
            var projectRepository = new ProjectController(mockManageProject.Object, Logger);

            var projectDetailsList = new List<Models.ProjectDetailModel>()
            {
                new Models.ProjectDetailModel() {ProjectId=1, Priority = 10},
                new Models.ProjectDetailModel() {ProjectId=2, Priority = 20},
            };

            mockManageProject.Setup(manage => manage.GetAllProjects()).Returns(Task.FromResult<IEnumerable<Models.ProjectDetailModel>>(projectDetailsList));

            // Act
            var statusResult = await projectRepository.GetProjects();

            // Assert
            Assert.NotNull(statusResult as OkObjectResult);

            var projectDetailsResult = (statusResult as OkObjectResult).Value as List<Models.ProjectDetailModel>;
            if (projectDetailsResult == null)
            {
                Assert.True(false, "Empty Result");
            }

            Assert.Equal(2, projectDetailsResult.Count);
        }

        [Fact]
        public async Task VerifyGetAllProjects_Throws_InternalServerErrorStatus_OnException()
        {
            // Arrange
            var mockManageProject = new Mock<IProjectManager>();
            var projectRepository = new ProjectController(mockManageProject.Object, Logger);

            mockManageProject.Setup(manage => manage.GetAllProjects()).Throws(new Exception());

            // Act
            var statusResult = await projectRepository.GetProjects();

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, (statusResult as ObjectResult)?.StatusCode);
        }


        [Fact]
        public async Task VerifyGetProjectDetail_Return_OkStatusAndprojectDetails()
        {
            // Arrange
            var mockManageProject = new Mock<IProjectManager>();
            var projectRepository = new ProjectController(mockManageProject.Object, Logger);

            var projectDetail = new Models.ProjectDetailModel() { ProjectId=1, Priority = 10 };

            mockManageProject.Setup(manage => manage.GetProjectDetail(1)).Returns(Task.FromResult(projectDetail));

            // Act
            var statusResult = await projectRepository.GetProjectbyId(1);

            // Assert
            Assert.NotNull(statusResult as OkObjectResult);

            var projectDetailsResult = (statusResult as OkObjectResult).Value as Models.ProjectDetailModel;
            Assert.Equal(1, projectDetailsResult?.ProjectId);
            Assert.Equal(10, projectDetailsResult?.Priority);
        }


        [Fact]
        public async Task VerifyGetProjectDetail_Throws_InternalServerErrorStatus_OnException()
        {
            //Arrange
            var mockManageProject = new Mock<IProjectManager>();
            var projectRepository = new ProjectController(mockManageProject.Object, Logger);

            mockManageProject.Setup(manage => manage.GetProjectDetail(1)).Throws(new Exception());

            //Act
            var statusResult = await projectRepository.GetProjectbyId(1);

            //Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, (statusResult as ObjectResult)?.StatusCode);
        }

        [Fact]
        public async Task VerifyAddProjectDetails_Returns_OkStatusAndCheckTaskId()
        {
            // Arrange
            var mockManageProject = new Mock<IProjectManager>();
            var projectRepository = new ProjectController(mockManageProject.Object, Logger);

            var projectDetail = new Models.ProjectDetailModel() { ProjectId=11, Priority = 10 };

            mockManageProject.Setup(manage => manage.AddProjectDetails(projectDetail)).Returns(Task.FromResult(1001));

            // Act
            var statusResult = await projectRepository.Post(projectDetail);

            // Assert
            Assert.NotNull(statusResult as OkObjectResult);

            Assert.Equal("Project with id 11 created successfully", (statusResult as OkObjectResult).Value);
        }

        [Fact]
        public async Task Verify_Project_Post_Returns_InternalServerErrorStatus_OnException()
        {
            // Arrange
            var mockManageProject = new Mock<IProjectManager>();
            var projectRepository = new ProjectController(mockManageProject.Object, Logger);
            var projectDetail = new Models.ProjectDetailModel() { ProjectId=12, Priority = 10 };
            mockManageProject.Setup(manage => manage.AddProjectDetails(projectDetail)).Throws(new Exception());

            // Act
            var statusResult = await projectRepository.Post(projectDetail);

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, (statusResult as ObjectResult)?.StatusCode);
        }

        [Fact]
        public async Task Verify_Project_Put_Return_OkStatusAndCheckServiceResponse()
        {
            // Arrange
            var mockManageProject = new Mock<IProjectManager>();
            var projectRepository = new ProjectController(mockManageProject.Object, Logger);
            var projectDetail = new Models.ProjectDetailModel() { ProjectId=1001, Priority = 10 };

            mockManageProject.Setup(manage => manage.IsProjectValid(projectDetail)).Returns(true);
            mockManageProject.Setup(manage => manage.UpdateProjectDetails(1001, projectDetail)).Returns(Task.FromResult(1001));

            // Act
            var statusResult = await projectRepository.Put(1001, projectDetail);

            // Assert
            Assert.NotNull(statusResult as OkObjectResult);

            Assert.Equal("Saved", (statusResult as OkObjectResult).Value);
        }

        [Fact]
        public async Task Verify_Project_Put_Returns_BadRequest_WhenprojectDetailIsInvalid()
        {
            // Arrange
            var mockManageProject = new Mock<IProjectManager>();
            var projectRepository = new ProjectController(mockManageProject.Object, Logger);
            var projectDetail = new Models.ProjectDetailModel() { ProjectId=1002, Priority = 10 };

            // Act
            var statusResult = await projectRepository.Put(1002, projectDetail);

            // Assert
            Assert.NotNull(statusResult as BadRequestObjectResult);
            Assert.Equal("This project has active tasks. Active tasks has to be closed before closing project", (statusResult as BadRequestObjectResult).Value);
        }

        [Fact]
        public async Task Verify_Project_Put_Returns_BadRequestWhenprojectDetailIsNotValidToClose()
        {
            // Arrange
            var mockManageProject = new Mock<IProjectManager>();
            var projectRepository = new ProjectController(mockManageProject.Object, Logger);
            var projectDetail = new Models.ProjectDetailModel() { ProjectId=1001, Priority = 10, EndProject = true };
            mockManageProject.Setup(manage => manage.IsProjectValid(projectDetail)).Returns(false);

            // Act
            var statusResult = await projectRepository.Put(1001, projectDetail);

            // Assert
            Assert.NotNull(statusResult as BadRequestObjectResult);            
        }

        [Fact]
        public async Task Verify_Project_Put_Returns_OkStatusWhenprojectDetailIsValidToClose()
        {
            // Arrange
            var mockManageProject = new Mock<IProjectManager>();
            var projectRepository = new ProjectController(mockManageProject.Object, Logger);

            var projectDetail = new Models.ProjectDetailModel() { ProjectId=1001, Priority = 10, EndProject = true };

            mockManageProject.Setup(manage => manage.IsProjectValid(projectDetail)).Returns(true);

            mockManageProject.Setup(manage => manage.UpdateProjectDetails(1001, projectDetail)).Returns(Task.FromResult(1001));

            // Act
            var statusResult = await projectRepository.Put(1001, projectDetail);

            // Assert
            Assert.NotNull(statusResult as OkObjectResult);

            Assert.Equal("Saved", (statusResult as OkObjectResult).Value);
        }

        [Fact]
        public async Task Verify_Project_Put_Returns_ReturnInternalServerErrorStatus_OnException()
        {
            // Arrange
            var mockManageProject = new Mock<IProjectManager>();
            var projectRepository = new ProjectController(mockManageProject.Object, Logger);
            var projectDetail = new Models.ProjectDetailModel() { ProjectId=1001, Priority = 10 };
            mockManageProject.Setup(manage => manage.IsProjectValid(projectDetail)).Returns(true);
            mockManageProject.Setup(manage => manage.UpdateProjectDetails(1001, projectDetail)).Throws(new Exception());

            // Act
            var statusResult = await projectRepository.Put(1001, projectDetail);

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, (statusResult as ObjectResult)?.StatusCode);
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
        }
        #endregion
    }
}
