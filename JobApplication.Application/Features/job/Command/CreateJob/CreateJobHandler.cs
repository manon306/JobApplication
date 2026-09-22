using JobApplication.Application.interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace JobApplication.Application.Features.job.Command.CreateJob
{
    public class CreateJobHandler : IRequestHandler<CreateJobCommand, int>
    {
        private readonly IJobRepository _jobRepository;
        private readonly IHttpContextAccessor _accessor;

        public CreateJobHandler(IJobRepository jobRepository, IHttpContextAccessor accessor)
        {
            _jobRepository = jobRepository;
            _accessor = accessor;
        }

        public async Task<int> Handle(CreateJobCommand request, CancellationToken cancellationToken)
        {
            var userID = _accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var job = new DataModel.Entities.job
            {
                title = request.CreateJobDTO.title,
                description = request.CreateJobDTO.description,
                RecruiterId = userID
            };
            await _jobRepository.CreateAsync(job);
            await _jobRepository.SaveChangesAsync();
            return job.id;
        }
    }
}
