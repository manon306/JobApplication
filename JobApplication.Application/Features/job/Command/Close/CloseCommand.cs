using MediatR;

namespace JobApplication.Application.Features.job.Command.Close
{
    public class CloseCommand :IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
