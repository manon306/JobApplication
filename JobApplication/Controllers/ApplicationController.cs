using JobApplication.Application.Features.ApplicationUser.Command.Apply;
using JobApplication.Application.Features.ApplicationUser.Command.CancleApp;
using JobApplication.Application.Features.ApplicationUser.Command.UpdateStatus;
using JobApplication.DataModel.Constants;
using JobApplication.DataModel.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ApplicationController( IMediator mediator)
        {
            _mediator = mediator;
        }
        [Authorize(Roles = Roles.Candidate)]
        [HttpPost("{jobId}/apply")]
        public async Task<IActionResult> Apply(int jobId)
        {
            var result = _mediator.Send(new ApplyCommand { jobId =  jobId });

            return Ok(result);
        }
        [Authorize(Roles = Roles.Candidate)]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> CancleApp(int id)
        {
            var result = _mediator.Send(new CancleAppCommand { Id = id });
            return Ok(result);
        }
        [Authorize(Roles = Roles.Recruiter)]
        [HttpPut("{applicationId}/status")]
        public async Task<IActionResult> UpdateStatus(int applicationId,[FromBody] jobApplicayionStatus newStatus)
        {
            var result = await _mediator.Send(new UpdateStatusCommand
            {
                applicationId = applicationId,
                newStatus = newStatus
            });

            return Ok();
        }

    }
}
