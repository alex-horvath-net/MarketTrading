namespace Design.Core.Enterprise.UserStory;

public class UserStoryCore_Design
{
    [Fact]
    public async void NonStoppedFeature()
    {
        var userStory = new UserStoryCore<RequestCore, ResponseCore<RequestCore>>(
            [
                oneTask.DoNotTerminate().Mock,
                otherTask.DoNotTerminate().Mock
            ]);

        var response = await userStory.Run(request, token);

        response.Should().NotBeNull();
        response.Request.Should().Be(request);
        response.Validations.Should().BeNull();
    }

    [Fact]
    public async void StoppedFeature()
    {
        var userStory = new UserStoryCore<RequestCore, ResponseCore<RequestCore>>(
            [
                oneTask.Terminate().Mock,
                otherTask.DoNotTerminate().Mock
            ]);

        var response = await userStory.Run(request, token);

        response.Should().NotBeNull();
        response.Request.Should().Be(request);

    }

    private readonly IUserTask_MockBuilder otherTask = new();
    private readonly IUserTask_MockBuilder oneTask = new();
    private readonly RequestCore request = new();
    private readonly CancellationToken token = CancellationToken.None;
}

