namespace IdentityService.Client;

public interface IIdentityClient {
    Task LogoutAsync(CancellationToken cancellationToken);
    Task<UserProfileResponse> GetProfileAsync(CancellationToken cancellationToken);
}
