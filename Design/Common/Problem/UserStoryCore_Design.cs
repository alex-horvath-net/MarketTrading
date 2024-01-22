using Common.Model;

namespace Common.Problem;

public class UserStoryCore_Design
{
    [Fact]
    public async void Provide_Response()
    {
        var userStory = new Story<Request, Response<Request>>();
            
        var response = await userStory.Run(request, token);

        response.Should().NotBeNull();
        response.Request.Should().Be(request);
        response.ValidationResults.Should().BeNull();
    }



    private readonly IUserTask_MockBuilder otherTask = new();
    private readonly IUserTask_MockBuilder oneTask = new();
    private readonly Request request = new();
    private readonly CancellationToken token = CancellationToken.None;
}

