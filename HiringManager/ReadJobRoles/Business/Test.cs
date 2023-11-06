using Shared;
using Shared.Business;
using Xunit;
using static HiringManager.ReadJobRoles.Adapters.Test;

namespace HiringManager.ReadJobRoles.Business;

public class Test
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

    public Request GetRequest(string? name = null) => new(Name: name ?? "Aladar");

    public CancellationToken GetToken() => CancellationToken.None;

    public Feature GetUnit(IRepository repository) => new(repository);

    private RepositoryMock GetRepositoryMock() => adapterTest.GetRepositoryMock();

    private readonly Adapters.Test adapterTest = new();
}