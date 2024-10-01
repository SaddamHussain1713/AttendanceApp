using AttendanceApp.Repositories;
using AttendanceApp.Services;
using AttendanceSystem.Data;
using AttendanceSystem.Domain;
using AttendanceSystem.Error;
using AttendanceSystem.Validators;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceApp.Controllers
{
    [ApiController]
    [Route("api[controller]")]
    public class ClassEntityController : Controller
    {
        private readonly IClassEntityService _service;

        public ClassEntityController(IClassEntityService classService)
        {
            _service = classService ?? throw new ArgumentNullException(nameof(classService));
        }
        [HttpPost("add-class")]
        public async Task<IActionResult> AddClass([FromBody] ClassEntity input)
        {
            var result = await _service.AddClassAsync(input);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.ErrorMessage);
        }
        [HttpPut("update-class")]
        public async Task<IActionResult> UpdateClass([FromBody] ClassEntity input)
        {
            var result = await _service.UpdateClassAsync(input);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result.ErrorMessage);
        }
        [HttpGet("get-class/{id}")]
        public async Task<IActionResult> GetClass(int id)
        {
            var result = await _service.GetClassByIdAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result.ErrorMessage);
        }
        [HttpDelete("delete-class/{id}")]
        public async Task<IActionResult> DeleteClass(int id)
        {
            var result = await _service.DeleteClassAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.ErrorMessage);
        }
        [HttpGet("get-all-classes")]
        public async Task<IActionResult> GetAllClasses()
        {
            var result = await _service.GetAllClassesAsync();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.ErrorMessage);
        }
    }
}
