using AppPolicy.UserStory.DomainModel;

namespace Design.Core.Sys.UserStory;

public class ValidationResult_Design {
    [Fact]
    public void ValidationResult_Success() {
        var result = Validation.Success();

        result.Should().NotBeNull();
        result.ErrorCode.Should().BeNull();
        result.ErrorMessage.Should().BeNull();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void ValidationResult_Failed() {
        var result = Validation.Failed("ErrorCode", "ErrorMessage");

        result.Should().NotBeNull();
        result.ErrorCode.Should().NotBeNull();
        result.ErrorMessage.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
    }
}
