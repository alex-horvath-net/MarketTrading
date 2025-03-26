//namespace Business.Experts.IdentityManager.LocalLogIn;

//public class Feature(IValidate validator, IEdit repository) {
//    public async Task<Response> Execute(Request request, CancellationToken token) {
//        var response = new Response();

//        response.Request = request;

//        response.Errors = await validator.Run(request, token);
//        if (response.Errors.Count > 0)
//            return response;

//        response.Transaction = await repository.EditTransaction(request, token);

//        return response;
//    }

//    public class Request {
//        public Guid Id { get; set; } = Guid.NewGuid();
//        public long TransactionId { get; set; }
//        public string Name { get; set; }
//        public string UserId { get; set; }
//    }

//    public class Response {
//        public Guid Id { get; set; } = Guid.NewGuid();
//        public bool Run { get; set; } = false;
//        public DateTime? StopedAt { get; set; }
//        public DateTime? FailedAt { get; internal set; }
//        public Exception? Exception { get; set; }
//        public Request? Request { get; set; }
//        public List<Error> Errors { get; set; } = [];
//        public Trade Transaction { get; set; }
//    }

//    public interface IValidate { Task<List<Error>> Run(Request request, CancellationToken token); }

//    public interface IEdit { Task<Trade> EditTransaction(Request request, CancellationToken token); }
//}

//public static class Extensions {
//    public static IServiceCollection AddEditTransaction(this IServiceCollection services, ConfigurationManager config) => services
//        .AddScoped<Feature>()
//        .AddValidator()
//        .AddEdit();
//}
