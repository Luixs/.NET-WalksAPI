using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Walks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllStudents()
        {

            List<string> studentsNames = new List<string>() { "Luis", "Guilherme", "Soares", "Starlino" };

            return Ok(studentsNames);
        }
    }
}
