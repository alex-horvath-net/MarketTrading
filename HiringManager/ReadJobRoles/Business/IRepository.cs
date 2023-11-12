using NSubstitute;
using Shared.Business;

namespace HiringManager.ReadJobRoles.Business
{
    public interface IRepository
    {
        Task<List<JobRole>> Read(Request request, CancellationToken token);

        public static class Mock
        {
            public static IRepository Simple()
            {
                var mock = Substitute.For<IRepository>();
                var response = new List<JobRole> 
                {
                    new JobRole("TestJobRole1"),
                    new JobRole("TestJobRole2"),
                    new JobRole("TestJobRole3")
                };
                mock.Read(default, default).ReturnsForAnyArgs(response);
                return mock;
            }
        }
    }
}