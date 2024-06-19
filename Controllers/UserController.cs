
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using YTCG_Deck_Builder_API.Data;
using YTCG_Deck_Builder_API.Dto;
using YTCG_Deck_Builder_API.Models.Entitities;
using YTCG_Deck_Builder_API.Services;

namespace YTCG_Deck_Builder_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<UserController> _logger;
        private readonly TokenGenerator _tokenGenerator;
        public UserController(DataContext dataContext, UserManager<User> userManager, SignInManager<User> signInManager, ILogger<UserController> logger, TokenGenerator tokenGenerator) {
            _dataContext = dataContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _tokenGenerator = tokenGenerator;
        }

        [HttpGet]
        public IActionResult GetAllUsers() {
            var allUsers = _dataContext.Users.ToList();
            return Ok(allUsers);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto userCreateDto)
        {
            var userExist = _dataContext.Users.Where(u => u.Email == userCreateDto.Email).ToList();

            if (userExist.Any())
            {
                ModelState.AddModelError("", "User already exists");
                return BadRequest(ModelState);
            }

            var newUser = new User()
            {
                UserName = userCreateDto.Username,
                Email = userCreateDto.Email,
                CreatedAt = DateTime.Now,
                Decks = [],
                Cards = [],
            };

            var result = await _userManager.CreateAsync(newUser, userCreateDto.Password);

            if (result.Succeeded)
            {
                return Ok(newUser);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginDto userLoginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _userManager.FindByEmailAsync(userLoginDto.Email);

                if (user == null)
                {
                    _logger.LogWarning("Login attempt failed: User not found.");
                    return BadRequest("User Not Found");
                }

                var result = await _signInManager.PasswordSignInAsync(user.UserName, userLoginDto.Password, false, false);
                if (result.Succeeded)
                {
                    var token = _tokenGenerator.GenerateToken(user);

                    // Set HttpOnly cookie
                    Response.Cookies.Append("token", token, new CookieOptions
                    {
                        SameSite = SameSiteMode.None,
                        Secure = true,
                        Path = "/",
                        IsEssential = true
                    });


                    return Ok(new {Token = token});
                }
                else
                {
                    _logger.LogWarning("Login attempt failed: Invalid credentials.");
                    return BadRequest("Login failed: Invalid credentials.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while logging in.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }


    }
}
