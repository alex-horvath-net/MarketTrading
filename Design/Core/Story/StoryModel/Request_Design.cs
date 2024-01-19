namespace Core.Story.StoryModel;

public class Request_Design {
    [Fact]
    public void Test_RequestCore() {
        var request = new Request();

        request = request with { };

        request.Should().NotBeNull();
        request.Should().BeOfType<Request>();
    }
}
