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
            var job = await _context.Jobs.FindAsync(jobId)
                ?? throw new KeyNotFoundException("Job not found.");

            if (!job.isActive) throw new InvalidOperationException("Job is closed.");

            var candidate = await _context.Candidates
                .FirstOrDefaultAsync(c => c.UserId == UserId);

            if (candidate == null)
            {
                throw new Exception("Candidate not found.");
            }

            if (await _context.Applications.AnyAsync(a => a.JobId == jobId && a.CandidateId == candidate.ID))
                throw new InvalidOperationException("Already applied.");
            

            var application = new DataModel.Entities.Application
            {
                JobId = jobId,
                CandidateId = candidate.ID,
                AppliedAt = DateTime.UtcNow,
                Status = DataModel.Enums.jobApplicayionStatus.Applied,
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
            if (application.Status is jobApplicayionStatus.Applied || application.Status is jobApplicayionStatus.UnderReview)
            {
                application.Status = jobApplicayionStatus.Canceled;
                application.CanceledAt = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();
        }
        private bool IsValidTransition(jobApplicayionStatus current, jobApplicayionStatus next)
        {
            return current switch
            {
                jobApplicayionStatus.Applied =>
                    next == jobApplicayionStatus.UnderReview,

                jobApplicayionStatus.UnderReview =>
                    next == jobApplicayionStatus.InterView,

                jobApplicayionStatus.InterView =>
                    next == jobApplicayionStatus.Accepted ||
                    next == jobApplicayionStatus.Rejected,

                _ => false
            };
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
            if (!IsValidTransition(application.Status, newStatus))
            {
                throw new InvalidOperationException(
                    $"Cannot change status from {application.Status} to {newStatus}.");
            }
            application.Status = newStatus;
             application.StatusUpdatedAt = DateTime.UtcNow;
             await _context.SaveChangesAsync();
            
        }
    }
}
