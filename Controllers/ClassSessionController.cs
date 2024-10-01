using AttendanceApp.Repositories;
using AttendanceSystem.Data;
using AttendanceSystem.Domain;
using AttendanceSystem.Validators;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceApp.Controllers
{
    [ApiController]
    [Route("api[controller]")]
    public class ClassSessionController : Controller
    {
        private readonly IClassSessionRepository _classSessionRepository;
        private readonly ClassSessionValidator _validator;

        public ClassSessionController(IClassSessionRepository classSessionRepository, ClassSessionValidator validator)
        {
            _classSessionRepository = classSessionRepository;
            _validator = validator;
        }

        [HttpPost("add-session")]
        public async Task<IActionResult> AddSession([FromBody] ClassSession session)
        {
            var validationResult = _validator.Validate(session);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var result = await _classSessionRepository.AddAsync(session);
            if (result.IsSuccess)
            {
                return Ok(result.Output);
            }
            return BadRequest(result.ErrorMessage);
        }
    }
}
