﻿using Common.Adapters.App.Data.Model;
using Common.Business.Model;
using Common.Technology.EF.App;
using DomainExperts.Trader.FindTransactions.UserStory.OutputPort;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DomainExperts.Trader.FindTransactions.WorkSteps;

public class Repository(Repository.IClient client) : IRepository {
    public async Task<List<Transaction>> FindTransactions(UserStory.InputPort.Request request, CancellationToken token) {
        var dataModel = await client.Find(request.Name, token);
        var businessModel = dataModel.Select(ToBusinessModel).ToList();

        token.ThrowIfCancellationRequested();
        return businessModel;
    }

    private static List<Transaction> ToBusinessModelList(List<TransactionDM> dataModelList) => dataModelList.Select(ToBusinessModel).ToList();
    private static Transaction ToBusinessModel(TransactionDM dataModel) => new() {
        Id = dataModel.Id,
        Name = dataModel.Name
    };


    public interface IClient {
        public Task<List<TransactionDM>> Find(string? name, CancellationToken token);
    }
    public class Client(AppDB db) : IClient {
        public async Task<List<TransactionDM>> Find(string? name, CancellationToken token) {
            token.ThrowIfCancellationRequested();

            var transactions = name == null ?
               await db.Transactions.AsNoTracking().ToListAsync(token) :
               await db.Transactions.AsNoTracking().Where(x => x.Name.Contains(name)).ToListAsync(token);

            token.ThrowIfCancellationRequested();

            return transactions;
        }
    }

}
