using JobApplication.Application.interfaces;
using JobApplication.DataModel.Entities;
using System;

namespace JobApplication.Application.Services.imp
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepo applicationRepo;
        public ApplicationService(IApplicationRepo applicationRepo)
        {
            this.applicationRepo = applicationRepo;
        }
        public async Task Apply(int jobId, string UserId)
        {
            await applicationRepo.Apply(jobId, UserId);
        }
        public async Task CancleApp(int id, string Userid)
        {
            await applicationRepo.CancleApp(id,Userid);
        }
        public async Task UpdateStatusAsync(int applicationId, DataModel.Enums.jobApplicayionStatus newStatus,string userId)
        {
            await applicationRepo.UpdateStatusAsync( applicationId, newStatus,userId);
        }
    }
}
