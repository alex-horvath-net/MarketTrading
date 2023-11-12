using HiringManager.ReadJobRoles.Business;

namespace HiringManager.ReadJobRoles.Adapters
{
    public class Validator : IValidator
    {
        public async Task<Result> Validate(Request request, CancellationToken token)
        {
            if (request == null) return Result.Failure( RequestErrors.RequestNull);

            if (request.Name == null) return Result.Failure(RequestErrors.NameNull); 
            
            return Result.Success();
        }
    }

    public static class RequestErrors
    {
        public static readonly Error RequestNull = new Error("Request", "Can not be null.");
        public static readonly Error NameNull = new Error("Request.Name", "Can not be null.");
    }
}
