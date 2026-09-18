using JobApplication.Application.DTOs;
using JobApplication.Application.interfaces;

namespace JobApplication.Application.Services
{
    public interface IJobServices
    {
        Task<int> CreateAsync(CreateJobDTO Dto, string userId);
        Task Close(int jobId, string userId);
    }
}
