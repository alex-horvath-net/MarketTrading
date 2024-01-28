namespace Common.Solutions.Data.MainDB.Configuration;
public sealed class TransientFaultHandlingOptions {
    public const string SectionName = nameof(TransientFaultHandlingOptions);

    public bool Enabled { get; set; }
    public TimeSpan AutoRetryDelay { get; set; }
}