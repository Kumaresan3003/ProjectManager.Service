namespace ProjectManager.Service.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using ProjectManager.Service.Business;
    using ProjectManager.Service.Models;

    [Produces("application/json")]
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserManager userManager, ILogger<UserController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        // GET: api/User
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                _logger.LogInformation("Get All Users");

                return Ok(await _userManager.GetAllUsers());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal Server error. Try again later");
            }
        }

        // GET: api/User/5
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                _logger.LogInformation($"Getting user details for {id}");

                return Ok(await _userManager.GetUserDetail(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal Server error. Try again later");
            }
        }

        // POST: api/User
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]UserDetailModel userDetailModel)
        {
            try
            {
                if (userDetailModel == null)
                {
                    _logger.LogInformation($"User is null.  Provide valid user details.");
                    return BadRequest($"User is null.  Provide valid user details.");
                }

                await _userManager.AddUserDetails(userDetailModel);

                _logger.LogInformation($"User has been added Successfully and the new user id is { userDetailModel.UserId }");

                return Ok(userDetailModel.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal Server error. Try again later");
            }
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]UserDetailModel userDetailModel)
        {
            try
            {
                _logger.LogInformation($"Updating user {id}");
                if (userDetailModel == null || id != userDetailModel.UserId)
                {
                    _logger.LogInformation("Invalid user to edit");
                    return BadRequest("Invalid user to edit");
                }

                await _userManager.UpdateUserDetails(id, userDetailModel);
                _logger.LogInformation($"User has been updated successfully for the user id { userDetailModel.UserId } ");
                return Ok(userDetailModel.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal Server error. Try again later");
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var user = await _userManager.GetUserDetail(id);
                if (!_userManager.IsUserValid(user))
                {
                    _logger.LogInformation("You can not delete as the user have association with Project/Task");
                    return BadRequest("You can not delete as the user have association with Project/Task");
                }

                await _userManager.RemoveUser(user);

                return Ok(user.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal Server error. Try again later");
            }
        }
    }
}
