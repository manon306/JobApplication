using JobApplication.DataModel.Entities;

namespace JobApplication.Application.interfaces
{
    public interface IJobRepository
    {
        Task CreateAsync(job job);
        Task SaveChangesAsync();

        Task Close(int jobId, string userId);
    }
}
