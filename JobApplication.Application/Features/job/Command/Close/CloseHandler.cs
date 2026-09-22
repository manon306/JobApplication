using JobApplication.Application.interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace JobApplication.Application.Features.job.Command.Close
{
    public class CloseHandler (IJobRepository jobRepository, IHttpContextAccessor accessor) : 
        IRequestHandler<CloseCommand, Unit>
    {
        private readonly IJobRepository _jobRepository = jobRepository;
        private readonly IHttpContextAccessor _accessor = accessor;

        public async Task<Unit> Handle(CloseCommand request, CancellationToken cancellationToken)
        {
            var userId = _accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _jobRepository.Close(request.Id, userId);
            await _jobRepository.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
