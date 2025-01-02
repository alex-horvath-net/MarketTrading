namespace Tests.Shared;

public static class TestConfig {
    static TestConfig() {
        var variableName = "MaxTestLevel";
        var defaultVariableValue = "Unit";
        var variableValue = Environment.GetEnvironmentVariable(variableName) ?? defaultVariableValue;
        MaxTestLevel = Enum.Parse<TestLevel>(value: variableValue, ignoreCase: true);
    }
    public static TestLevel MaxTestLevel;
}
