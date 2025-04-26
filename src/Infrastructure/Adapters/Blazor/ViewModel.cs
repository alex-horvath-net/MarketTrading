namespace Infrastructure.Adapters.Blazor;

public record ViewModel {
    public MetaVM Meta { get; set; }
    public IEnumerable<ErrorVM> Errors { get; set; } = [];


}

public class ErrorVM {
    public string Name { get; set; }
    public string Message { get; set; }
}


public class MetaVM {
    public Guid Id { get;  set; }
}

