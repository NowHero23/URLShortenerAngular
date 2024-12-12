using Microsoft.AspNetCore.Mvc;
using URLShortenerAngular.Server.Data.Domain.Repositories.Abstract;
using URLShortenerAngular.Server.Data.Dtos;
using URLShortenerAngular.Server.Models;
using URLShortenerAngular.Server.Services;

namespace URLShortenerAngular.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;
        public AuthController(IUserRepository repository, JwtService jwtService)
        {
            _userRepository = repository;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            if (_userRepository.GetByLogin(dto.Login) != null) return BadRequest(new { message = "The user already exists" });
            var user = new User
            {
                Login = dto.Login,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                RegisterDate = DateTime.UtcNow,
                AccessLevel = Data.Enums.AccessLevelEnum.user
            };

            return Created("Success", _userRepository.Create(user));
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {

            var user = _userRepository.GetByLogin(dto.Login);
            if (user == null) return BadRequest(new { message = "Invalid Credentials" });

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                return BadRequest(new { message = "Invalid Credentials" });
            }

            var jwt = _jwtService.Generate(user.Id);

            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true
            });

            return Ok(new
            {
                message = "Success"
            });
        }


        [HttpGet("user")]
        public IActionResult GetUser()
        {
            try
            {
                string jwt = Request.Cookies["jwt"];

                if (jwt == null)
                    return Unauthorized();


                var token = _jwtService.Varify(jwt);

                int userId = int.Parse(token.Issuer);

                var user = _userRepository.GetById(userId);
                return Ok(user);
            }
            catch (Exception e)
            {
                return Unauthorized();
            }
        }


        [HttpPost("logout")]
        public IActionResult Logout()
        {
            string jwt = Request.Cookies["jwt"];

            if (jwt == null)
                return Unauthorized();

            Response.Cookies.Delete("jwt");
            return Redirect("/");
        }
    }
}
