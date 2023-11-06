using Common;
using Domain;
using HiringManager.ReadJobRoles.Business;

namespace Design.HiringManager.ReadJobRoles.Business;

public class Feature
{
    [Fact]
    public async Task Can_Read_JobRoles()
    {
        var request = GetRequest();
        var token = GetToken();
        var repository = GetRepositoryMock();
        var unit = GetUnit(repository);

        var response = await unit.Run(request, token);

        response.JobRoles.ShouldBe_NotEmpty();
    }

    [Fact]
    public async Task The_Read_Particular_JobRoles()
    {
        var request = GetRequest();
        var token = GetToken();
        var repository = GetRepositoryMock();
        var unit = GetUnit(repository);

        var response = await unit.Run(request, token);

        response.JobRoles[0].Name.ShouldBe(request.Name);
    }

    [Fact]
    public async Task The_Read_JobRoles_By_Repository()
    {
        var request = GetRequest();
        var token = GetToken();
        var repository = GetRepositoryMock();
        var unit = GetUnit(repository);

        await unit.Run(request, token);

        repository.Counter.ShouldBe(1);
    }

    public Request GetRequest(string? name = null)
    {
        var request = new Request(
           Name: name ?? "Aladar");
        return request;
    }

    public CancellationToken GetToken()
    {
        var token = CancellationToken.None;
        return token;
    }

    public HiringManager.ReadJobRoles.Business.Feature GetUnit(IRepository repository)
    {
        var unit = new HiringManager.ReadJobRoles.Business.Feature(repository);
        return unit;
    }

    private RepositoryMock GetRepositoryMock()
    {
        var repository = new RepositoryMock();
        return repository;
    }
    public class RepositoryMock : IRepository
    {
        public int Counter { get; private set; }
        public Task<List<JobRole>> Add(Request request)
        {
            Counter++;
            return new List<JobRole>() { new("Aladar") }.ToTask();
        }
    }
}