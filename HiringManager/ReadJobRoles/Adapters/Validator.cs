using HiringManager.ReadJobRoles.Business;

namespace HiringManager.ReadJobRoles.Adapters;

public class Validator : IValidator
{
    public async Task<Error> Validate(Request request, CancellationToken token) =>
        request == null ? Result.RequestNull :
        request.Name == null ? Result.NameNull :
        Result.Success;


}

public static class Result
{
    public static readonly Error Success = Error.None;
    public static readonly Error RequestNull = new Error("Request", "Can not be null.");
    public static readonly Error NameNull = new Error("Request.Name", "Can not be null.");
}
