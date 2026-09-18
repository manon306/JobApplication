using JobApplication.Application.DTOs;
using JobApplication.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class jobController : ControllerBase
    {
        private readonly IJobServices _jobServices;
        public jobController(IJobServices jobServices)
        {
            _jobServices = jobServices;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateJob([FromBody] CreateJobDTO Dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var jobId = await _jobServices.CreateAsync(Dto,userId);
            return CreatedAtAction(nameof(CreateJob), new { id = jobId }, null);
        }
        [Authorize]
        [HttpPost]
        [Route("{jobId}/close")]
        public async Task<IActionResult> Close(int jobId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _jobServices.Close(jobId,userId);
            return Ok();
        }
    }
}
