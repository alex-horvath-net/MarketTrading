using Core.Business.Model;

namespace Experts.Blogger.ReadPosts.Business.Model;
public record Request(string Filter) : RequestCore() { }
