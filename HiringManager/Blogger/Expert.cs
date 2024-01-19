using Experts.Blogger.ReadPosts;
using Experts.Blogger.ReadPosts.Model;
using Microsoft.Extensions.DependencyInjection;
using Story;

namespace Experts.Blogger;

public record Expert(Story<Request, Response> ReadPosts);


public static class Extensions {
    public static IServiceCollection AddBlogger(this IServiceCollection services) => services
        .AddScoped<Expert>()
        .AddScoped<Story<Request, Response>, Strory>()

            .AddProblem<
                ReadPosts.Validation.Problem,
                ReadPosts.Validation.ISolution,
                ReadPosts.Validation.Solution>()

            .AddProblem<
                ReadPosts.Read.Problem,
                ReadPosts.Read.ISolution,
                ReadPosts.Read.Solution>()
        ;

    private static IServiceCollection AddProblem<P, R, S>(this IServiceCollection services)
        where P : class, IProblem<Request, Response>
        where R : class
        where S : class, R => services

           .AddScoped<IProblem<Request, Response>, P>()
           .AddScoped<R, S>();
}

