namespace Tests;
public class IntegrationFact : FactAttribute {
    public IntegrationFact() {

        if (TestConfig.SkipIntegrationTests) {
            Skip = "Integration tests are skipped based on the environment variable.";
        }
    }
}

public static class TestConfig {
    static TestConfig() => SkipIntegrationTests = bool.Parse(Environment.GetEnvironmentVariable("SkipIntegrationTests") ?? "true");

    public static bool SkipIntegrationTests;
}
