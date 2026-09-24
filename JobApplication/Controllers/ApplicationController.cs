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
    public class ApplicationController (IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        
        /// <summary>
        /// Submits a job application for the specified job on behalf of the authenticated candidate.
        /// </summary>
        /// <param name="jobId">The unique identifier of the job to apply for.</param>
        /// <returns>A success response indicating the application was submitted.</returns>
        [Authorize(Roles = Roles.Candidate)]
        [HttpPost("{jobId}/apply")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Apply(int jobId)
        {
            var result = await _mediator.Send(new ApplyCommand { jobId =  jobId });

            return Ok(result);
        }

        /// <summary>
        /// Cancels an existing job application submitted by the authenticated candidate.
        /// </summary>
        /// <param name="id">The unique identifier of the job application to cancel.</param>
        /// <returns>A success response confirming the application cancellation.</returns>
        [Authorize(Roles = Roles.Candidate)]
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CancleApp(int id)
        {
            var result = await _mediator.Send(new CancleAppCommand { Id = id });
            return Ok(result);
        }

        /// <summary>
        /// Updates the status of a job application for a job owned by the authenticated recruiter.
        /// </summary>
        /// <param name="applicationId">The unique identifier of the job application to update.</param>
        /// <param name="newStatus">The new application status to assign.</param>
        /// <returns>An empty response indicating the application status was successfully updated.</returns>
        [Authorize(Roles = Roles.Recruiter)]
        [HttpPut("{applicationId}/status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStatus(int applicationId,[FromBody] jobApplicayionStatus newStatus)
        {
            await _mediator.Send(new UpdateStatusCommand
            {
                applicationId = applicationId,
                newStatus = newStatus
            });

            return Ok();
        }

    }
}
