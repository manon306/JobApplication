using JobApplication.DataModel.Enums;
using MediatR;

namespace JobApplication.Application.Features.ApplicationUser.Command.UpdateStatus
{
    public class UpdateStatusCommand : IRequest<Unit>
    {
        public int applicationId {  get; set; }
        public jobApplicayionStatus newStatus { get; set; }

    }
}
