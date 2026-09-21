using MediatR;

namespace JobApplication.Application.Features.ApplicationUser.Command.Apply
{
    public class ApplyCommand :IRequest<Unit>
    {
        public int jobId { get; set; }
    }
}
