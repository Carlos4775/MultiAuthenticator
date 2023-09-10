using AutoMapper;
using BusinessLogic.Services.AuthenticationService;
using BusinessLogic.Services.TokensService;
using Data.Models;
using Data.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MultiAuthenticator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AuthController(ITokenService tokenService, IAuthenticationService authenticationService, IMapper mapper)
        {
            _tokenService = tokenService;
            _authenticationService = authenticationService;
            _mapper = mapper;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            User mappedUser = _mapper.Map<User>(userLogin);

            string error = await _authenticationService.ValidateData(mappedUser);

            if (!string.IsNullOrEmpty(error))
            {
                ModelState.AddModelError("Errors", error);
                return BadRequest(ModelState);
            }

            Token token = _tokenService.CreateToken(mappedUser);

            mappedUser.RefreshToken = token.RefreshToken;
            mappedUser.RefreshTokenEndDate = token.Expiration.AddMinutes(5);

            return Ok(token);
        }

        [HttpPost("refreshToken")]
        public async Task<IActionResult> Login(RefreshToken refreshToken)
        {
            User user = await _authenticationService.GetByRefreshTokenAsync(refreshToken.Token);

            if (user == null)
            {
                return BadRequest(new { message = "refresh token is invalid." });
            }

            if (user.RefreshTokenEndDate < DateTime.Now)
            {
                return BadRequest(new { message = "refresh token expired" });
            }

            Token token = _tokenService.CreateToken(user);

            user.RefreshToken = token.RefreshToken;
            user.RefreshTokenEndDate = token.Expiration.AddMinutes(5);

            return Ok(token);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserLogin userLogin)
        {
            if (userLogin?.Username == null || userLogin?.Email == null || userLogin?.Password == null)
            {
                return BadRequest(new { message = "An error occurred. Some information was missing." });
            }

            // Check if a user with the same username already exists
            if (await _authenticationService.GetByUsernameAsync(userLogin.Username) != null)
            {
                return BadRequest(new { message = "User with this username already exists." });
            }

            // Check if a user with the same email already exists
            if (await _authenticationService.GetByEmailAsync(userLogin.Email) != null)
            {
                return BadRequest(new { message = "User with this email already exists." });
            }

            // Hash the password
            string hashedPassword = _authenticationService.HashPassword(userLogin.Password);

            // Create a new user
            User newUser = new()
            {
                Username = userLogin.Username,
                Email = userLogin.Email,
                Password = hashedPassword,
                Address = userLogin.Address,
                Gender = userLogin.Gender,
                Phone = userLogin.Phone,
                FirstName = userLogin.FirstName,
                LastName = userLogin.LastName,
                RoleId = userLogin.RoleId
            };

            // Create a token for the new user
            Token token = _tokenService.CreateToken(newUser);

            // Update the user with refresh token information
            newUser.RefreshToken = token.RefreshToken;
            newUser.RefreshTokenEndDate = token.Expiration.AddMinutes(5);

            // Save the user to the database
            await _authenticationService.PostAsync(newUser);

            return Ok(token);
        }
    }
}