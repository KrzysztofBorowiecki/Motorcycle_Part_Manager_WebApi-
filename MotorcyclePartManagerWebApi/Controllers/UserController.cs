using Microsoft.AspNetCore.Mvc;
using MotorcyclePartManagerWebApi.Models;
using MotorcyclePartManagerWebApi.Services.UserServices;
using System.Threading.Tasks;

namespace MotorcyclePartManagerWebApi.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IAddUserService _addUserService;
        private readonly IJwtTokenGeneratorService _jwtToken;

        public UserController(IAddUserService addUserService, IJwtTokenGeneratorService jwtToken)
        {
            _addUserService = addUserService;
            _jwtToken = jwtToken;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Login model)
        {
            string token = _jwtToken.Generate(model);

            return  Ok(token);
        }

        [HttpPost("singup")]
        public async Task<IActionResult> Singup([FromBody] Singup model)
        {
            await _addUserService.AddUserAsync(model);

            return Ok();
        }
    }
}
