using JobApplication.Application.interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace JobApplication.Application.Features.job.Command.Close
{
    public class CloseHandler : IRequestHandler<CloseCommand, Unit>
    {
        private readonly IJobRepository _jobRepository;
        private readonly IHttpContextAccessor _accessor;

        public CloseHandler(IJobRepository jobRepository, IHttpContextAccessor accessor)
        {
            _jobRepository = jobRepository;
            _accessor = accessor;
        }

        public async Task<Unit> Handle(CloseCommand request, CancellationToken cancellationToken)
        {
            var userId = _accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _jobRepository.Close(request.Id, userId);
            await _jobRepository.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
