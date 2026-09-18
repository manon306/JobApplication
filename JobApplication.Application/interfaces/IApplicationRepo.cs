using JobApplication.DataModel.Enums;

namespace JobApplication.Application.interfaces
{
    public interface IApplicationRepo
    {
        Task Apply(int jobId, string UserId);
        Task CancleApp(int id, string Userid);
        Task UpdateStatusAsync(int applicationId, jobApplicayionStatus newStatus, string userId);
    }
}
