//using HiringManager.ReadJobRoles.Adapters;
//using Shared;
//using Xunit;

//namespace HiringManager.ReadJobRoles.Business;

//public class Test_Feature_Constructor
//{
//    public void Act() => unit = new Feature(repository);

//    [Fact]
//    public void Construct()
//    {
//        repository = Adapters.Specify.CreateRepositoryDeafaultMock();
//        Act();
//        unit.ShouldBe_NotNull();
//    }

//    private Feature unit;
//    private IRepository repository;
//}

//public class Test_Feature_Run
//{
//    [Fact]
//    public async Task Can_Read_JobRoles()
//    {
//        var request = GetRequest();
//        var token = GetToken();
//        var repository = GetRepositoryMock();
//        var unit = CreateFeature(repository);

//        var response = await unit.Run(request, token);

//        response.Posts.ShouldBe_Not_Empty();
//    }

//    [Fact]
//    public async Task The_Read_Particular_JobRoles()
//    {
//        var request = GetRequest();
//        var token = GetToken();
//        var repository = GetRepositoryMock();
//        var unit = CreateFeature(repository);

//        var response = await unit.Run(request, token);

//        response.Posts[0].Name.ShouldBe(request.Name);
//    }

//    [Fact]
//    public async Task The_Read_JobRoles_By_Repository()
//    {
//        var feature = UseMocks().CreateUnit();
//        var request = GetRequest();
//        var token = GetToken();

//        await feature.Run(request, token);

//        repository.ShouldBe_NotNull();
//    }

//    public Request GetRequest(string? name = null) => new(Name: name ?? "Aladar");

//    public CancellationToken GetToken() => CancellationToken.None;

//    public Feature CreateUnit() => new Feature(repository);

//    public Specify UseMocks()
//    {
//        repository = adapterTest.CreateRepositoryDeafaultMock();
//        return this;
//    }

//    private IRepository repository;

//    private readonly Adapters.Specify adapterTest = new();
//}