using HiringManager.ReadJobRoles.Adapters;
using Shared;
using Xunit;

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
        var repository = GetRepositoryMock();
        var unit = GetUnit(repository);
        var request = GetRequest();
        var token = GetToken();

        await unit.Run(request, token);

        repository.ShouldBe_NotNull();
    }

    public Request GetRequest(string? name = null) => new(Name: name ?? "Aladar");

    public CancellationToken GetToken() => CancellationToken.None;

    public Feature GetUnit(IRepository repository) => new(repository);

    private IRepository GetRepositoryMock() => adapterTest.GetRepositoryMock();

    private readonly Adapters.Test adapterTest = new();
}