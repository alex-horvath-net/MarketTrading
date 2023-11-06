using Shared.Business;

namespace HiringManager.ReadJobRoles.Business
{
    public interface IRepository
    {
        Task<List<JobRole>> Add(Request request);
    }
}