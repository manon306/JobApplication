using JobApplication.Application.DTOs;
using MediatR;

namespace JobApplication.Application.Features.job.Command.CreateJob
{
    public class CreateJobCommand :IRequest<int>
    {
        public CreateJobDTO CreateJobDTO { get; set; }
    }
}
