using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Authorization;

namespace MotorcyclePartManagerWebApi.Controllers
{
    [Route("file")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class FileController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        [ResponseCache(Duration = 2000, VaryByQueryKeys =new[] { "fileName" })]
        public async Task<IActionResult> GetFile([FromQuery] string fileName)
        {
            var rootPath = Directory.GetCurrentDirectory();
            var filePath = $"{rootPath}/PrivateFiles/{fileName}";

            var fileExist = System.IO.File.Exists(filePath);

            if (!fileExist)
            {
                return NotFound();
            }

            var contentProvider = new FileExtensionContentTypeProvider();
            contentProvider.TryGetContentType(fileName, out string contentType);

           var fileContent = await System.IO.File.ReadAllBytesAsync(filePath);

            return File(fileContent, contentType, fileName);
        }

        [HttpPost]
        public ActionResult Upload([FromForm] IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var rootPath = Directory.GetCurrentDirectory();
                var fileName = file.FileName;
                var fullPath = $"{rootPath}/PrivateFiles/{fileName}";
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                } 

                return Ok();
            }

            return BadRequest();
        }
    }
}
