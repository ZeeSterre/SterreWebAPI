using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SterreWebApi.Models;
using SterreWebApi.Repositorys;

namespace SterreWebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Userinfo")]
    public class UserInfoController : ControllerBase
    {
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly IAuthenticationService _authenticationService;

        public UserInfoController(IUserInfoRepository userInfoRepository, IAuthenticationService authenticationService)
        {
            _userInfoRepository = userInfoRepository;
            _authenticationService = authenticationService;
        }

        // GET: /Userinfo
        [HttpGet]
        [Authorize(Policy = "UserEntity")]
        public async Task<ActionResult<IEnumerable<Environment2DDTO>>> GetEnvironments()
        {
            try
            {
                var userIdString = _authenticationService.GetCurrentAuthenticatedUserId();
                if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
                {
                    return Unauthorized("User is not authenticated.");
                }

                var environments = await _userInfoRepository.GetEnvironmentsUser(userId);
                var environmentDtos = environments.Select(e => new Environment2DDTO
                {
                    Id = e.Id,
                    Name = e.Name,
                    MaxLength = e.MaxLength,
                    MaxHeight = e.MaxHeight,
                    EnvironmentType = e.EnvironmentType
                });

                return Ok(environmentDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching user environments: {ex.Message}");
            }
        }

        // GET: /Userinfo/{environmentId}
        [HttpGet("{environmentId}")]
        [Authorize(Policy = "UserEntity")]
        public async Task<ActionResult<Environment2DDTO>> GetEnvironmentById(Guid environmentId)
        {
            try
            {
                var userIdString = _authenticationService.GetCurrentAuthenticatedUserId();
                if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
                {
                    return Unauthorized("User is not authenticated.");
                }

                var environment = await _userInfoRepository.GetEnvironmentById(environmentId, userId);
                if (environment == null)
                {
                    return NotFound("Environment not found or does not belong to the user.");
                }

                var environmentDto = new Environment2DDTO
                {
                    Id = environment.Id,
                    Name = environment.Name,
                    MaxLength = environment.MaxLength,
                    MaxHeight = environment.MaxHeight,
                    EnvironmentType = environment.EnvironmentType
                };

                return Ok(environmentDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching the environment: {ex.Message}");
            }
        }
        // POST: /environment2d
        [HttpPost]
        [Authorize(Policy = "UserEntity")]
        public async Task<IActionResult> CreateEnvironment([FromBody] CreateEnvironmentRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Invalid request data.");
                }
                var userIdString = _authenticationService.GetCurrentAuthenticatedUserId();
                if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
                    return Unauthorized("User is not authenticated.");

                var newEnvironment = new Environment2D
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    MaxLength = 100,
                    MaxHeight = 50,
                    UserId = userId,
                    EnvironmentType = request.EnvironmentType
                };

                var success = await _userInfoRepository.CreateEnvironment(newEnvironment);
                if (!success)
                    return StatusCode(500, "An error occurred while creating the environment.");

                return CreatedAtAction(nameof(GetEnvironments), new { id = newEnvironment.Id }, newEnvironment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching user environments: {ex.Message}");
            }
        }

        // DELETE: /environment2d/{environmentId}
        [HttpDelete("{environmentId}")]
        [Authorize(Policy = "UserEntity")]
        public async Task<IActionResult> DeleteEnvironment(Guid environmentId)
        {
            try
            {
                if (environmentId == Guid.Empty)
                {
                    return BadRequest("Invalid environment ID.");
                }

                var userId = _authenticationService.GetCurrentAuthenticatedUserId();

                if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var parsedUserId))
                {
                    return Unauthorized("User is not authenticated.");
                }

                var result = await _userInfoRepository.DeleteEnvironment(environmentId, parsedUserId);

                if (result)
                {
                    return Ok("Environment deleted successfully.");
                }
                else
                {
                    return NotFound("Environment not found or does not belong to the user.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the environment: {ex.Message}");
            }
        }


        // GET: /object2d/{environment2dId}/all
        [HttpGet("{environmentId}/allObjects")]
        [Authorize(Policy = "UserEntity")]
        public async Task<ActionResult<IEnumerable<Object2D>>> GetAllObjects(Guid environmentId)
        {
            try
            {
                var userIdString = _authenticationService.GetCurrentAuthenticatedUserId();
                if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
                    return Unauthorized("User is not authenticated.");

                var environment = await _userInfoRepository.GetEnvironmentById(environmentId, userId);
                if (environment == null)
                    return NotFound("Environment not found or does not belong to the user.");

                var objects = await _userInfoRepository.GetObjectsByEnvironmentId(environmentId);

                if (objects == null || !objects.Any())
                    return NotFound("No objects found for this environment.");

                return Ok(objects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching objects: {ex.Message}");
            }
        }

        //GET: /object2d/{objectId}
        [HttpGet("GetObject/{objectId}")]
        [Authorize(Policy = "UserEntity")]
        public async Task<ActionResult<Object2D>> GetObjectById(Guid objectId)
        {
            try
            {
                var userIdString = _authenticationService.GetCurrentAuthenticatedUserId();
                if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
                    return Unauthorized("User is not authenticated.");
                var object2D = await _userInfoRepository.GetObjectById(objectId);
                if (object2D == null)
                    return NotFound("Object not found.");
                return Ok(object2D);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching the object: {ex.Message}");
            }
        }

        // POST: /object2d/createObject
        [HttpPost("createObject")]
        [Authorize(Policy = "UserEntity")]
        public async Task<ActionResult<Object2D>> CreateObject([FromBody] CreateObject2DRequest createRequest)
        {
            try
            {
                var userIdString = _authenticationService.GetCurrentAuthenticatedUserId();
                if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
                    return Unauthorized("User is not authenticated.");

                var environment = await _userInfoRepository.GetEnvironmentById(createRequest.Environment2D_Id, userId);
                if (environment == null)
                    return NotFound("Environment not found or does not belong to the user.");

                var newObject2D = new Object2D
                {
                    Id = Guid.NewGuid(),
                    PrefabId = createRequest.PrefabId,
                    PositionX = createRequest.PositionX,
                    PositionY = createRequest.PositionY,
                    ScaleX = createRequest.ScaleX,
                    ScaleY = createRequest.ScaleY,
                    RotationZ = createRequest.RotationZ,
                    SortingLayer = createRequest.SortingLayer,
                    Environment2D_Id = createRequest.Environment2D_Id
                };

                var result = await _userInfoRepository.CreateObject(newObject2D);

                if (result == 0)
                    return BadRequest("Failed to create Object2D.");

                return CreatedAtAction(nameof(GetAllObjects), new { environmentId = newObject2D.Environment2D_Id }, newObject2D);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the object: {ex.Message}");
            }
        }

        // DELETE: /object2d/{objectId}
        [HttpDelete("deleteObject/{objectId}")]
        [Authorize(Policy = "UserEntity")]
        public async Task<IActionResult> DeleteObject(Guid objectId)
        {
            try
            {
                var userIdString = _authenticationService.GetCurrentAuthenticatedUserId();
                if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
                    return Unauthorized("User is not authenticated.");
                var object2D = await _userInfoRepository.GetObjectById(objectId);
                if (object2D == null)
                    return NotFound("Object not found.");
                var result = await _userInfoRepository.DeleteObject(objectId);
                if (result)
                    return Ok("Object deleted successfully.");
                else
                    return NotFound("Object not found or does not belong to the user.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the object: {ex.Message}");
            }
        }
    }
}
