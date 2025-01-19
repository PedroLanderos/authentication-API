using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationApiController(IUser authenticationInterface) : ControllerBase
    {
        [HttpPost("Register")]
        public async Task<ActionResult<Response>> Register(UserDTO appUserDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await authenticationInterface.Register(appUserDTO);
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<Response>> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await authenticationInterface.Login(loginDTO);
            return result.Flag ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            if (id <= 0)
                return BadRequest("invalid id");
            var user = await authenticationInterface.GetUser(id);

            if (user is not null) return Ok(user);
            else return NotFound();

        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            var users = await authenticationInterface.GetAllUsers();
            return users.Any() ? Ok(users) : NotFound("No users found");
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> EditUser(UserDTO appUserDTO)
        {
            if (appUserDTO.Id <= 0)
                return BadRequest("Invalid user ID");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await authenticationInterface.EditUserById(appUserDTO);
            return result.Flag ? Ok(result) : BadRequest(result);
        }
    }
}
