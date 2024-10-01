using AttendanceApp.Services;
using AttendanceSystem.Data;
using AttendanceSystem.Domain;
using AttendanceSystem.Repositories;
using AttendanceSystem.Validators;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AttendanceApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : Controller
    {
        private readonly IStudentService _service;
        public StudentController(IStudentService service)
        {
            _service = service;
        }
        // Change the endpoint to use POST and use async Task<IActionResult>
        [HttpPost("add-student")]
        public async Task<IActionResult> AddStudent([FromBody] Student input)
        {
            var result = await _service.AddStudentAsync(input);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.ErrorMessage);
        }
        [HttpGet("get-student/{id}")]
        public async Task<IActionResult> GetStudent(int id)
        {
            var result = await _service.GetStudentByIdAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.ErrorMessage);
        }
        [HttpDelete("delete-student/{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var result = await _service.DeleteStudentAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.ErrorMessage);
        }
        [HttpPut("update-student")]
        public async Task<IActionResult> UpdateStudent([FromBody] Student input)
        {
            var result = await _service.UpdateStudentAsync(input);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.ErrorMessage);
        }
        [HttpGet("get-all-students")]
        public async Task<IActionResult> GetAllStudents()
        {
            var result = await _service.GetAllStudentsAsync();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.ErrorMessage);
        }
    }
}
