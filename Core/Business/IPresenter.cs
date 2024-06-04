using Azure;
using Core.Business.Model;

namespace Core.Business;

public interface IPresenter<TRequest, TResponse>
  where TRequest : RequestCore
  where TResponse : ResponseCore<TRequest>, new() {
    void MapUS2UI(TResponse response);
}

 