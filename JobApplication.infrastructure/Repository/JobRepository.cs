using JobApplication.Application.interfaces;
using JobApplication.DataModel.Entities;
using JobApplication.infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace JobApplication.infrastructure.Repository
{
    public class JobRepository (ApplicationDBContext context) : IJobRepository
    {
        private readonly ApplicationDBContext _context = context;
        public async Task CreateAsync(job job)
        {
            await _context.Jobs.AddAsync(job);
        }
        
        public async Task Close(int jobId,string userId)
        {
            
            var job = await _context.Jobs.FindAsync(jobId) ?? throw new Exception($"Job with ID {jobId} not found."); ;
            
            if (job.RecruiterId != userId)
            {
                throw new UnauthorizedAccessException(
                    "You are not allowed to close this job.");
            }
            if (!job.isActive)
                throw new InvalidOperationException(
                    "Job is already closed.");
            job.isActive = false;
            job.ClosedAt = DateTime.UtcNow;
            job.ClosedByID = userId; 
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
