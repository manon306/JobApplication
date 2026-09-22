using JobApplication.Application.interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Features.ApplicationUser.Command.UpdateStatus
{
    public class UpdateStatusHandler : IRequestHandler<UpdateStatusCommand, Unit>
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IApplicationRepo _appRepo;

        public UpdateStatusHandler(IApplicationRepo appRepo, IHttpContextAccessor accessor)
        {
            _appRepo = appRepo;
            _accessor = accessor;
        }
        public async Task<Unit> Handle(UpdateStatusCommand request, CancellationToken cancellationToken)
        {
            var userId = _accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _appRepo.UpdateStatusAsync(request.applicationId, request.newStatus, userId);
            return Unit.Value;
        }
    }
}
