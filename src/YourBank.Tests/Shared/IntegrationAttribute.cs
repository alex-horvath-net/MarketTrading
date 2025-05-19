namespace YourBank.Tests.Shared;
public class IntegrationFactAttribute : FactAttribute {
    public IntegrationFactAttribute() {
        if (TestConfig.MaxTestLevel < TestLevel.Integration)
            Skip = $"Integration test is skipped the MaxTestLevel is {TestConfig.MaxTestLevel}.";
    }
}
