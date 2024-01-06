using Core.Enterprise.UserStory;
using Design.Core.Enterprise;
using Users.Blogger.ReadPostsUserStory;
using static Users.Blogger.ReadPostsUserStory.ValidationUserTask.UserTask;
using SUT = Users.Blogger.ReadPostsUserStory.ValidationUserTask.UserTask;

namespace Design.Users.Blogger.ReadPostsUserStory.ValidationUserTask;

public class ValidationTask_Design : Design<SUT>
{
    private void Construct() => Unit = new(validationSocket);

    private async Task Run() => await Unit.Run(response, Token);

    [Fact]
    public void ItHas_Sockets()
    {
        Construct();

        Unit.Should().NotBeNull();
        Unit.Should().BeAssignableTo<IUserTask<global::Users.Blogger.ReadPostsUserStory.Request, Response>>();
    }

    [Fact]
    public async void ItCan_ValidateValidRequest()
    {
        mockValidationSocket.Pass();
        Construct();
        mockResponse.HasNoValidations();

        await Run();

        mockResponse.Mock.Terminated.Should().BeFalse();
        mockResponse.Mock.Validations.Should().NotContain(x => !x.IsSuccess);
        mockResponse.Mock.Validations.Should().BeEmpty();
        await mockValidationSocket.Mock.ReceivedWithAnyArgs().Validate(default, default);
    }

    [Fact]
    public async void ItCan_ValidateInValidRequest()
    {
        mockValidationSocket.Fail();
        Construct();
        mockResponse.HasNoValidations();

        await Run();

        mockResponse.Mock.Terminated.Should().BeTrue();
        mockResponse.Mock.Validations.Should().Contain(x => !x.IsSuccess);
        await mockValidationSocket.Mock.ReceivedWithAnyArgs().Validate(default, default);
    }

    private readonly IValidationSocket_MockBuilder mockValidationSocket = new();
    private IValidationSocket validationSocket => mockValidationSocket.Mock;
    private readonly Response_MockBuilder mockResponse = new();
    private Response response => mockResponse.Mock;


    public ValidationTask_Design(ITestOutputHelper output) : base(output) { }
}