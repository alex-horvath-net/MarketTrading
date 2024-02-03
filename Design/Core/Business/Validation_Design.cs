namespace Core.Business;

public class Validation_Design
{
    [Fact]
    public void ValidationResult_Success()
    {
        var result = new Model.Success();

        result.Should().NotBeNull();
        result.ErrorCode.Should().BeNull();
        result.ErrorMessage.Should().BeNull();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void ValidationResult_Failed()
    {
        var result = new Model.Failed("ErrorCode", "ErrorMessage");

        result.Should().NotBeNull();
        result.ErrorCode.Should().NotBeNull();
        result.ErrorMessage.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
    }
}
