namespace Core.Business;

public class Request_Design
{
    [Fact]
    public void Test_RequestCore()
    {
        var request = new RequestCore();

        request = request with { };

        request.Should().NotBeNull();
        request.Should().BeOfType<RequestCore>();
    }
}
