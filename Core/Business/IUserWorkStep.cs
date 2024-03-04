using System.ComponentModel.DataAnnotations;
using Azure.Core;
using Core.Business.Model;
using Core.Solutions.Validation;

namespace Core.Business;

public interface IUserWorkStep<TRequest, TResponse, TSettings>
    where TRequest : RequestCore
    where TResponse : ResponseCore<TRequest>, new()
    where TSettings : SettingsCore {
    Task<bool> Run(TResponse response, CancellationToken token);
}

public class StartUserWorkStep<TRequest, TResponse, TSettings>(
    ILog log,
    ITime time) : IUserWorkStep<TRequest, TResponse, TSettings>
    where TRequest : RequestCore
    where TResponse : ResponseCore<TRequest>, new()
    where TSettings : SettingsCore {

    public Task<bool> Run(TResponse response, CancellationToken token) {
        response.MetaData.StartedAt = time.Now;
        return true.ToTask();
    }
}

public class FeatureActivationUserWorkStep<TRequest, TResponse, TSettings>(
    ISettings<TSettings> settings,
    ILog log,
    ITime time) : IUserWorkStep<TRequest, TResponse, TSettings>
    where TRequest : RequestCore
    where TResponse : ResponseCore<TRequest>, new()
    where TSettings : SettingsCore {

    public async Task Run(TResponse response, CancellationToken token) {
        response.MetaData.Enabled = settings.Value.Enabled;
        if (!response.MetaData.Enabled) response.MetaData.Stoped = time.Now;
    }


    public class StopUserWorkStep<TRequest, TResponse, TSettings>(
        ILog log,
        ITime time) : IUserWorkStep<TRequest, TResponse, TSettings>
        where TRequest : RequestCore
        where TResponse : ResponseCore<TRequest>, new()
        where TSettings : SettingsCore {

        public async Task Run(TResponse response, CancellationToken token) =>
            response.MetaData.Stoped = time.Now;

    }
}

public class ValidationUserWorkStep<TRequest, TResponse, TSettings>(
    IValidator<TRequest> validator,
    ILog log,
    ITime time) : IUserWorkStep<TRequest, TResponse, TSettings>
    where TRequest : RequestCore
    where TResponse : ResponseCore<TRequest>, new()
    where TSettings : SettingsCore {

    public async Task Run(TResponse response, CancellationToken token) {
        response.MetaData.RequestIssues = await validator.Validate(response.MetaData.Request, token);
        if (response.MetaData.RequestIssues.HasFailed()) response.MetaData.Stoped = time.Now;
    }


    public class StopUserWorkStep<TRequest, TResponse, TSettings>(
        ILog log,
        ITime time) : IUserWorkStep<TRequest, TResponse, TSettings>
        where TRequest : RequestCore
        where TResponse : ResponseCore<TRequest>, new()
        where TSettings : SettingsCore {

        public async Task Run(TResponse response, CancellationToken token) =>
            response.MetaData.Stoped = time.Now;

    }
}

public class StopUserWorkStep<TRequest, TResponse, TSettings>(
    ILog log,
    ITime time) : IUserWorkStep<TRequest, TResponse, TSettings>
    where TRequest : RequestCore
    where TResponse : ResponseCore<TRequest>, new()
    where TSettings : SettingsCore {

    public async Task Run(TResponse response, CancellationToken token) =>
        response.MetaData.Stoped = await time.Now.ToTask();
}

