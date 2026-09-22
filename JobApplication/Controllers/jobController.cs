using JobApplication.Application.DTOs;
using JobApplication.Application.Features.job.Command.Close;
using JobApplication.Application.Features.job.Command.CreateJob;
using JobApplication.DataModel.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobApplication.API.Controllers
{
    [Route("api/job")]
    [ApiController]
    public class JobController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _Mediator = mediator;
        
        /// <summary>
        /// Creates a new job posting for the authenticated recruiter.
        /// </summary>
        /// <param name="Dto">The data transfer object containing job details such as title and description.</param>
        /// <returns>The unique identifier of the newly created job.</returns>
        [Authorize(Roles = Roles.Recruiter)]
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateJob([FromBody] CreateJobDTO Dto)
        {
            var result =await _Mediator.Send(new CreateJobCommand { CreateJobDTO = Dto });
            return Ok(result);
        }
        /// <summary>
        /// Closes an active job posted by the authenticated recruiter.
        /// </summary>
        /// <param name="jobId">The unique identifier of the job to close.</param>
        /// <returns>No content if the job is successfully closed.</returns>
        [Authorize(Roles = Roles.Recruiter)]
        [HttpPut]
        [Route("{jobId}/close")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> Close(int jobId)
        {
            await _Mediator.Send(new CloseCommand { Id = jobId});
            return NoContent();
        }
    }
}
