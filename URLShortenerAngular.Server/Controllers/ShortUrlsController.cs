using Microsoft.AspNetCore.Mvc;
using URLShortenerAngular.Server.Data.Domain.Repositories.Abstract;
using URLShortenerAngular.Server.Data.Dtos;
using URLShortenerAngular.Server.Models;
using URLShortenerAngular.Server.Services;

namespace URLShortenerAngular.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShortUrlsController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IUrlItemRepository _urlRepository;
        private readonly JwtService _jwtService;
        public ShortUrlsController(IUserRepository userRepository, JwtService jwtService, IUrlItemRepository urlRepository)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _urlRepository = urlRepository;
        }

        [HttpPost("create")]
        public IActionResult Create(UrlCreateDto dto)
        {
            try
            {
                string jwt = Request.Cookies["jwt"];

                if (jwt == null)
                    return Unauthorized();

                var token = _jwtService.Varify(jwt);

                int userId = int.Parse(token.Issuer);

                var user = _userRepository.GetById(userId);

                UrlItem url = new UrlItem
                {
                    ShortUrl = dto.ShortUrl,
                    OriginalUrl = dto.OriginalUrl,
                    TransitionsCount = 0,
                    CreatedDate = DateTime.UtcNow,
                    AuthorId = userId,
                    //Author = user
                };

                if (_urlRepository.GetAll().Any((u) => u.ShortUrl == dto.ShortUrl || u.OriginalUrl == dto.OriginalUrl))
                {
                    return BadRequest(new { message = "The short Url already exists" });
                }
                return Created("Success", _urlRepository.Create(url));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Invalid Credentials" });
            }
        }

        [HttpGet("table")]
        public IActionResult GetUrlsTable()
        {
            return Ok(_urlRepository.GetAll().ToArray());
        }

        [HttpGet("table/{id}")]
        public IActionResult GetUrlDetails(string id)
        {
            int num;
            if (int.TryParse(id, out num))
            {
                var url = _urlRepository.GetDetailsById(num);

                if (url != null)
                    return Ok(url);
            }
            return NotFound();
        }

        [HttpDelete("table/{id}")]
        public IActionResult DeleteUrl(string id)
        {
            int num;
            if (!int.TryParse(id, out num))
            {
                return BadRequest(new { message = "Invalid Credentials" });
            }
            UrlItem? url;
            try
            {
                string jwt = Request.Cookies["jwt"];

                if (jwt == null)
                    return Unauthorized();

                var token = _jwtService.Varify(jwt);

                int userId = int.Parse(token.Issuer);

                var user = _userRepository.GetById(userId);
                url = _urlRepository.GetDetailsById(num);
                if (url == null)
                    BadRequest(new { message = "The short Url is not exists" });

                if (user.Id != url?.AuthorId && user.AccessLevel != Data.Enums.AccessLevelEnum.admin)
                    return BadRequest(new { message = "You are not the author of the short Url" });

            }
            catch (Exception e)
            {
                return BadRequest(new { message = "Invalid Credentials" });
            }

            if (_urlRepository.Delete(url))
                return Ok("Success");

            return BadRequest(new { message = "Something went wrong" });
        }
    }
}
