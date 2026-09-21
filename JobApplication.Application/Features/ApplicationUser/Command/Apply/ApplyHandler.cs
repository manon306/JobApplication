using JobApplication.Application.interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Features.ApplicationUser.Command.Apply
{
    public class ApplyHandler : IRequestHandler<ApplyCommand, Unit>
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IApplicationRepo _appRepo;

        public ApplyHandler(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public async Task<Unit> Handle(ApplyCommand request, CancellationToken cancellationToken)
        {
            var userId = _accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _appRepo.Apply(request.jobId, userId);
            return Unit.Value;
        }
    }
}
