using Shared.Business;

namespace HiringManager.ReadJobRoles.Business
{
    public interface IRepository
    {
        Task<List<JobRole>> Read(Request request, CancellationToken token);
        int SomeProperty { get; set; }
    }
}