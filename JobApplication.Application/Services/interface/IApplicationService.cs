using JobApplication.DataModel.Enums;
using System;
using System.Collections.Generic;
namespace JobApplication.Application.Services
{
    public interface IApplicationService
    {
        Task Apply(int jobId, string UserId);
        Task CancleApp(int id, string Userid);
        Task UpdateStatusAsync(int applicationId, jobApplicayionStatus newStatus, string userId);
    }
}
