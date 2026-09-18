using JobApplication.Application.Services;
using JobApplication.Application.Services.imp;
using JobApplication.DataModel.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;
        public ApplicationController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }
        [Authorize]
        [HttpPost("{jobId}/apply")]
        public async Task<IActionResult> Apply(int jobId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _applicationService.Apply(jobId, userId!);

            return Ok();
        }
        [Authorize]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> CancleApp(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId == null)
            {
                return Unauthorized();
            }
            await _applicationService.CancleApp(id,userId);
            return Ok();
        }
        [Authorize]
        [HttpPut("{applicationId}/status")]
        public async Task<IActionResult> UpdateStatus(int applicationId,[FromBody] jobApplicayionStatus newStatus)
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            await _applicationService.UpdateStatusAsync(
                applicationId,
                newStatus,
                userId!);

            return Ok();
        }

    }
}
