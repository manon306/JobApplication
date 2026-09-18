using JobApplication.Application.interfaces;
using JobApplication.DataModel.Enums;
using JobApplication.infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JobApplication.infrastructure.Repository
{
    public class ApplicationRepo : IApplicationRepo
    {
        private readonly ApplicationDBContext _context;
        public ApplicationRepo(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task Apply(int jobId, string UserId)
        {
            var candidate = await _context.Candidates
                .FirstOrDefaultAsync(c => c.UserId == UserId);

            if (candidate == null)
            {
                throw new Exception("Candidate not found.");
            }
            var application = new DataModel.Entities.Application
            {
                JobId = jobId,
                CandidateId = candidate.ID,
                AppliedAt = DateTime.UtcNow,
                Status = DataModel.Enums.jobApplicayionStatus.UnderReview,
                StatusUpdatedAt = DateTime.UtcNow
            };
            await _context.Applications.AddAsync(application);
            await _context.SaveChangesAsync();
        }
        public async Task CancleApp(int id , string Userid)
        {
            var application = await _context.Applications
                .Include(a => a.Candidate)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (application == null)
            {
                throw new Exception("Application not found.");
            }
            if (application.Candidate.UserId != Userid)
            {
                throw new UnauthorizedAccessException();
            }
            if (application != null && (application.Status is jobApplicayionStatus.Applied || application.Status is jobApplicayionStatus.UnderReview))
            {
                _context.Applications.Remove(application);
            }
            await _context.SaveChangesAsync();
        }
        public async Task UpdateStatusAsync(int applicationId,jobApplicayionStatus newStatus,string userId)
        {
            var application = await _context.Applications
                .Include(a=>a.job)
                .FirstOrDefaultAsync(a =>a.Id == applicationId);
            if(application == null)
            {
                throw new Exception("Application not found.");
            }
            if(application.job.RecruiterId != userId)
            {
                throw new UnauthorizedAccessException();
            }
             application.Status = newStatus;
             application.StatusUpdatedAt = DateTime.UtcNow;
             await _context.SaveChangesAsync();
            
        }
    }
}
