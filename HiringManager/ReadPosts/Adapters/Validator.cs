using Blogger.ReadPosts.Business;

namespace Blogger.ReadPosts.Adapters;

public class Validator : IValidator
{
    public async Task<Error> Validate(Request request, CancellationToken token) =>
        request == null ? Result.RequestNull :
        Result.Success;


}

public static class Result
{
    public static readonly Error Success = Error.None;
    public static readonly Error RequestNull = new Error("Request", "Can not be null.");
}
