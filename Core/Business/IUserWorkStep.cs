using System.ComponentModel.DataAnnotations;
using Azure;
using Azure.Core;
using Core.Business.Model;
using Core.Solutions.Validation;

namespace Core.Business;

public interface IUserWorkStep<TRequest, TResponse>
    where TRequest : RequestCore
    where TResponse : ResponseCore<TRequest>, new() {
    Task<bool> Run(TResponse response, CancellationToken token);
    string Name { get; }
}


public abstract class UserWorkStep<TRequest, TResponse> : IUserWorkStep<TRequest, TResponse>
    where TRequest : RequestCore where TResponse : ResponseCore<TRequest>, new() {

    public virtual Task<bool> Run(TResponse response, CancellationToken token) {
        return true.ToTask();
    }
    public string Name => name ??= GetType().Name;
    private string name;
}

public class StartUserWorkStep<TRequest, TResponse>(ITime time) :
    UserWorkStep<TRequest, TResponse>
    where TRequest : RequestCore where TResponse : ResponseCore<TRequest>, new() {

    public Task<bool> Run(TResponse response, CancellationToken token) {
        response.MetaData.StartedAt = time.Now;
        return true.ToTask();
    }
}

public class FeatureActivationUserWorkStep<TRequest, TResponse, TSettings>(ISettings<TSettings> settings) :
    UserWorkStep<TRequest, TResponse>
    where TRequest : RequestCore where TResponse : ResponseCore<TRequest>, new() where TSettings : SettingsCore {

    public Task<bool> Run(TResponse response, CancellationToken token) {
        response.MetaData.Enabled = settings.Value.Enabled;
        return response.MetaData.Enabled.ToTask();
    }
}


public class StopUserWorkStep<TRequest, TResponse, TSettings>(ITime time) :
    UserWorkStep<TRequest, TResponse>
    where TRequest : RequestCore where TResponse : ResponseCore<TRequest>, new() {

    public Task<bool> Run(TResponse response, CancellationToken token) {
        response.MetaData.Stoped = time.Now;
        return true.ToTask();
    }
}

