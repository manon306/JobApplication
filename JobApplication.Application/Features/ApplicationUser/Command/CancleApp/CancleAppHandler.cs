using JobApplication.Application.interfaces;
using JobApplication.Application.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Application.Features.ApplicationUser.Command.CancleApp
{
    public class CancleAppHandler : IRequestHandler<CancleAppCommand, Unit>
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IApplicationRepo _appRepo;
        private readonly IBackgroundJobScheduler _backgroundJob;
        private readonly INotificationService notificationService;
        public CancleAppHandler(IApplicationRepo appRepo, IHttpContextAccessor accessor, IBackgroundJobScheduler backgroundJob, INotificationService notificationService)
        {
            _appRepo = appRepo;
            _accessor = accessor;
            _backgroundJob = backgroundJob;
            this.notificationService = notificationService;
        }

        public async Task<Unit> Handle(CancleAppCommand request, CancellationToken cancellationToken)
        {
            var userId = _accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _appRepo.CancleApp(request.Id, userId);
            _backgroundJob.Enqueue<INotificationService>(b => b.NotifyCandidate(request.Id));
            return Unit.Value;
        }
    }
}
