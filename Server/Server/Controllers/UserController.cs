using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Server.Bll.Interfaces;
using Server.Models;
using Server.Models.DTO;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public IMapper Mapper { get; }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                var token = await _userService.Login(loginDto.Username, loginDto.Password);
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred: ");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            try
            {
                var user = _mapper.Map<User>(registerDto);

                await _userService.Register(user);
                return Ok("User registered successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred: ");
            }
        }
        [HttpGet("{username}")]
        public async Task<IActionResult> UsernameExist(string username)
        {
            try
            {
                return Ok(await _userService.UsernameExist(username));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred: ");
            }

        }
    }
}
