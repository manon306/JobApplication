using JobApplication.Application.DTOs;
using JobApplication.Application.interfaces;
using JobApplication.DataModel.Entities;
using System.IO.Pipes;

namespace JobApplication.Application.Services.imp
{
    public class JobServices : IJobServices
    {
        private readonly IJobRepository _jobRepository;

        public JobServices(IJobRepository jobRepository)
        {
            _jobRepository = jobRepository;
        }
        public async Task<int> CreateAsync(CreateJobDTO Dto, string userId)
        {
            var job = new job
            {
                title = Dto.title,
                description = Dto.description,
                RecruiterId = userId
            };
            await _jobRepository.CreateAsync(job);
            await _jobRepository.SaveChangesAsync();
            return job.id;

        }
        
        public async Task Close(int jobId, string userId)
        {
            await _jobRepository.Close(jobId,userId);
            await _jobRepository.SaveChangesAsync();
        }
    }
}
