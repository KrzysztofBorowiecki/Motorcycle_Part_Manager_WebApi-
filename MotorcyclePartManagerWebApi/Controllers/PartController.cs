using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotorcyclePartManagerWebApi.Entities;
using MotorcyclePartManagerWebApi.Models;
using MotorcyclePartManagerWebApi.Repositories;
using System.Threading.Tasks;

namespace MotorcyclePartManagerWebApi.Controllers
{
    [ApiController]
    [Route("api/part")]
    [Authorize(Roles = "User")]
    public class PartController : ControllerBase
    {
        private readonly IPartRepository _partRepository;
        public PartController(IPartRepository partRepository)
        {
            _partRepository = partRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllParts([FromQuery] PartQuery query)
        {
            var result = await _partRepository.GetPartsAsync(query);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPart([FromRoute] int id)
        {
            var result = await _partRepository.GetPartAsync(id);

            //if (result is null)
            //{
            //    return NotFound();
            //}

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddPart([FromRoute] Part part)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _partRepository.AddPartAsync(part);

            return Created($"/api/motorcycle/{part.Id}", null);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePart([FromRoute] Part part)
        {
            await _partRepository.UpdatePartAsync(part);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePart([FromRoute] int id)
        {
            await _partRepository.DeletePartAsync(id);

            return Ok();
        }
    }
}
