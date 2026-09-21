using JobApplication.Application.DTOs;
using JobApplication.Application.Features.job.Command.Close;
using JobApplication.Application.Features.job.Command.CreateJob;
using JobApplication.Application.Services;
using JobApplication.DataModel.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobApplication.API.Controllers
{
    [Route("api/job")]
    [ApiController]
    public class jobController : ControllerBase
    {
        private readonly IMediator _Mediator;
        public jobController(IMediator mediator)
        {
            _Mediator = mediator;
        }

        [Authorize(Roles = Roles.Recruiter)]
        [HttpPost]
        public async Task<IActionResult> CreateJob([FromBody] CreateJobDTO Dto)
        {
            var result =await _Mediator.Send(new CreateJobCommand { CreateJobDTO = Dto });
            return Ok(result);
        }
        [Authorize(Roles = Roles.Recruiter)]
        [HttpPut]
        [Route("{jobId}/close")]
        public async Task<IActionResult> Close(int jobId)
        {
            var result = await _Mediator.Send(new CloseCommand { Id = jobId});
            return NoContent();
        }
    }
}
