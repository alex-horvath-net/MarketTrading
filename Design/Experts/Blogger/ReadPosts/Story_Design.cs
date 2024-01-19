using Experts.Blogger.ReadPosts.Model;

namespace Experts.Blogger.ReadPosts;

public static class RequestExtensions {
    public static Request MockValidRequest(this Request request) =>
        new Request("Title", "Content");

    public static Request MockMissingpProperties(this Request request) =>
        request = new Request(null, null);

    public static Request MockTooShortProperties(this Request request) =>
        new Request("12", "21");
}

public static class ResponseExtensions {
    public static Response MockNoPosts(this Response response) {
        response.MockValidRequest();
        response.Posts = null;
        return response;
    }

    public static Response MockValidRequest(this Response response) {
        response.Request = Request.Empty().MockValidRequest();
        response.FeatureEnabled = true;
        response.Validations = null;
        return response;
    }

    public static Response MockNoValidations(this Response response) {
        response.MockValidRequest();
        response.Validations = null;
        return response;
    }
}
