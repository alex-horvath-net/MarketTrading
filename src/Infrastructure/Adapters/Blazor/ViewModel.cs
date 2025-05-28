
namespace Infrastructure.Adapters.Blazor;

public record ViewModel {
    public MetaVM Meta { get; set; }
    public List<ErrorVM> Errors { get; set; } = [];

    public string? Result { get; set; }
    public string AlertCssClass { get; set; } 


}

public class ErrorVM {
    public string Name { get; set; }
    public string Message { get; set; }
}


public class MetaVM {
    public TimeSpan Duration { get; set; }
}

