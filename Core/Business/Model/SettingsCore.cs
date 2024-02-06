namespace Core.Business.Model;
public record SettingsCore(string SectionName) {
    public bool Enabled { get; set; }
}
