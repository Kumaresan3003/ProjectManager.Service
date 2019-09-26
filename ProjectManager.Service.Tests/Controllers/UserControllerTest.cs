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

    public class UserControllerTest:IDisposable
    {       
        public ILogger<UserController> Logger { get; }

        public UserControllerTest()
        {
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            Logger = factory.CreateLogger<UserController>();
        }

        [Fact]
        public async Task VerifyGetAllUsers_Returns_TwoUserDetails()
        {
            // Arrange
            var mockManageUser = new Mock<IUserManager>();
            var userRepository = new UserController(mockManageUser.Object, Logger);

            var userDetailsList = new List<Models.UserDetailModel>()
            {
                new Models.UserDetailModel() {UserId= 1, FirstName ="Test1",LastName="one",EmployeeId=100},
                new Models.UserDetailModel() {UserId = 2, FirstName ="Test2", LastName="two",EmployeeId=200},
            };

            mockManageUser.Setup(manage => manage.GetAllUsers()).Returns(Task.FromResult<IEnumerable<Models.UserDetailModel>>(userDetailsList));

            // Act
            var statusResult = await userRepository.GetAllUsers();

            // Assert
            Assert.NotNull(statusResult as OkObjectResult);

            var userDetailsResult = (statusResult as OkObjectResult).Value as List<Models.UserDetailModel>;
            if (userDetailsResult == null)
            {
                Assert.True(false, "Empty Result");
            }

            Assert.Equal(2, userDetailsResult.Count);
        }

        [Fact]
        public async Task VerifyGetAllUsers_Throws_InternalServerErrorStatus_OnException()
        {
            // Arrange
            var mockManageUser = new Mock<IUserManager>();
            var userRepository = new UserController(mockManageUser.Object, Logger);

            mockManageUser.Setup(manage => manage.GetAllUsers()).Throws(new Exception());

            // Act
            var statusResult = await userRepository.GetAllUsers();

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, (statusResult as ObjectResult)?.StatusCode);
        }


        [Fact]
        public async Task VerifyGetUserDetail_Return_OkStatusAndUserDetails()
        {
            // Arrange
            var mockManageUser = new Mock<IUserManager>();
            var userRepository = new UserController(mockManageUser.Object, Logger);

            var userDetail = new Models.UserDetailModel() { UserId = 1, EmployeeId = 10 };

            mockManageUser.Setup(manage => manage.GetUserDetail(1)).Returns(Task.FromResult(userDetail));

            // Act
            var statusResult = await userRepository.GetUserById(1);

            // Assert
            Assert.NotNull(statusResult as OkObjectResult);

            var userDetailsResult = (statusResult as OkObjectResult).Value as Models.UserDetailModel;
            Assert.Equal(1, userDetailsResult?.UserId);
            Assert.Equal(10, userDetailsResult?.EmployeeId);
        }


        [Fact]
        public async Task VerifyGetUserDetail_Throws_InternalServerErrorStatus_OnException()
        {
            //Arrange
            var mockManageUser = new Mock<IUserManager>();
            var userRepository = new UserController(mockManageUser.Object, Logger);

            mockManageUser.Setup(manage => manage.GetUserDetail(1)).Throws(new Exception());

            //Act
            var statusResult = await userRepository.GetUserById(1);

            //Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, (statusResult as ObjectResult)?.StatusCode);
        }

        [Fact]
        public async Task VerifyAddUserDetails_Returns_OkStatusAndCheckTaskId()
        {
            // Arrange
            var mockManageUser = new Mock<IUserManager>();
            var userRepository = new UserController(mockManageUser.Object, Logger);

            var userDetail = new Models.UserDetailModel() { UserId = 1001, EmployeeId = 10 };

            mockManageUser.Setup(manage => manage.AddUserDetails(userDetail)).Returns(Task.FromResult(1001));

            // Act
            var statusResult = await userRepository.Post(userDetail);

            // Assert
            Assert.NotNull(statusResult as OkObjectResult);

            Assert.Equal(1001, (statusResult as OkObjectResult).Value);
        }

        [Fact]
        public async Task Verify_User_Post_Returns_InternalServerErrorStatus_OnException()
        {
            // Arrange
            var mockManageUser = new Mock<IUserManager>();
            var userRepository = new UserController(mockManageUser.Object, Logger);
            var userDetail = new Models.UserDetailModel() { UserId = 1001,EmployeeId = 10 };
            mockManageUser.Setup(manage => manage.AddUserDetails(userDetail)).Throws(new Exception());

            // Act
            var statusResult = await userRepository.Post(userDetail);

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, (statusResult as ObjectResult)?.StatusCode);
        }

        [Fact]
        public async Task Verify_User_Put_Return_OkStatusAndCheckServiceResponse()
        {
            // Arrange
            var mockManageUser = new Mock<IUserManager>();
            var userRepository = new UserController(mockManageUser.Object, Logger);
            var userDetail = new Models.UserDetailModel() { UserId = 1001, EmployeeId = 10 };

            mockManageUser.Setup(manage => manage.IsUserValid(userDetail)).Returns(true);
            mockManageUser.Setup(manage => manage.UpdateUserDetails(1001, userDetail)).Returns(Task.FromResult(1001));

            // Act
            var statusResult = await userRepository.Put(1001, userDetail);

            // Assert
            Assert.NotNull(statusResult as OkObjectResult);

            Assert.Equal(1001, (statusResult as OkObjectResult).Value);
        }

        [Fact]
        public async Task Verify_User_Put_Returns_BadRequest_WhenuserDetailIsInvalid()
        {
            // Arrange
            var mockManageUser = new Mock<IUserManager>();
            var userRepository = new UserController(mockManageUser.Object, Logger);
            var userDetail = new Models.UserDetailModel() { UserId = 1001, EmployeeId = 10 };

            // Act
            var statusResult = await userRepository.Put(1002, userDetail);

            // Assert
            Assert.NotNull(statusResult as BadRequestObjectResult);
            Assert.Equal("Invalid user to edit", (statusResult as BadRequestObjectResult).Value);
        }

        [Fact]
        public async Task Verify_User_Put_Returns_ReturnInternalServerErrorStatus_OnException()
        {
            // Arrange
            var mockManageUser = new Mock<IUserManager>();
            var userRepository = new UserController(mockManageUser.Object, Logger);
            var userDetail = new Models.UserDetailModel() {UserId = 1001, EmployeeId= 10 };
            mockManageUser.Setup(manage => manage.IsUserValid(userDetail)).Returns(true);
            mockManageUser.Setup(manage => manage.UpdateUserDetails(1001, userDetail)).Throws(new Exception());

            // Act
            var statusResult = await userRepository.Put(1001, userDetail);

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
