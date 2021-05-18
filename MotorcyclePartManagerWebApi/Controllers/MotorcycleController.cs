using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotorcyclePartManagerWebApi.Entities;
using MotorcyclePartManagerWebApi.Repositories;
using System.Threading.Tasks;
using System.Security.Claims;
using MotorcyclePartManagerWebApi.Models;

namespace MotorcyclePartManagerWebApi.Controllers
{
    [ApiController]
    [Route("api/motorcycle")]
    [Authorize(Roles = "User")]
    public class MotorcycleController : ControllerBase
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        public MotorcycleController(IMotorcycleRepository motorcycleRepository)
        {
            _motorcycleRepository = motorcycleRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllMotorcycles()
        {
            var result = await _motorcycleRepository.GetMotorcyclesAsync();

            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetMotorcycle([FromRoute] int id)
        {
            var result = await _motorcycleRepository.GetMotorcycleAsync(id);

            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> AddMotorcycle([FromRoute] Motorcycle motorcycle)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _motorcycleRepository.AddMotorcycleAsync(motorcycle);

            return Created($"/api/motorcycle/{motorcycle.Id}", null);
        }


        [HttpPut] 
        public async Task<IActionResult> UpdateMotorcycle([FromRoute] Motorcycle motorcycle)
        {
            await _motorcycleRepository.UpdateMotorcycleAsync(motorcycle);

            return Ok();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMotorcycle([FromRoute] int id)
        {
           await _motorcycleRepository.DeleteMotorcycleAsync(id);

           return NoContent();
        }
    }
}
