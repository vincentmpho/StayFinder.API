using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StayFinder.API.Models.DTOs.Users;
using StayFinder.API.Services.Interface;

namespace StayFinder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthManager _authManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAuthManager authManager, ILogger<AccountController> logger)
        {
            _authManager = authManager;
            _logger = logger;
        }

        //POST:api/account/register
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<IActionResult> Register ([FromBody] ApiUserDto apiUserDto)
        {
            _logger.LogInformation($"Registration Attempt for {apiUserDto.Email}");

            try
            {
                var errors = await _authManager.Register(apiUserDto);

                if (errors.Any())
                {
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }
                return StatusCode(StatusCodes.Status200OK, "Registration successful. Welcome to our platform!");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(Register)}");
                //return Problem($"something Went wrong in the {nameof(Register)}", statusCode: 500);
                return StatusCode(StatusCodes.Status500InternalServerError, $"Something went wrong in the {nameof(Register)}");
            }
        }
        
        
        //POST:api/account/Login
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<IActionResult> Login ([FromBody] LoginDto loginDto)
        {

            _logger.LogInformation($"Login Atempt for {loginDto.Email}");
            try
            {
                var authResponse = await _authManager.Login(loginDto);

                if (authResponse == null)
                {
                    return Unauthorized();
                }
                return StatusCode(StatusCodes.Status200OK, authResponse);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(Login)}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Something Went Wrong in  the {nameof(Login)}");
                
            }

        }

        //POST:api/account/refreshtoken
        [HttpPost]
        [Route("refreshtoken")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Refreshtoken([FromBody] AuthResponseDto request)
        {
            try
            {
                var authResponse = await _authManager.VeryifyRefreshToken(request);

                if (authResponse == null)
                {
                    return Unauthorized();
                }

                return StatusCode(StatusCodes.Status200OK, authResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(Refreshtoken)}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Something Went Wrong in  the {nameof(Refreshtoken)}");
            }
        }
    }
}
