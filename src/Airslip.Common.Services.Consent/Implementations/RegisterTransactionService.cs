using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Services.Consent.Entities;
using Airslip.Common.Services.Consent.Interfaces;
using Airslip.Common.Services.Consent.Models;
using Airslip.Common.Types.Enums;
using Airslip.Common.Utilities;
using Airslip.Common.Utilities.Extensions;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Airslip.Common.Services.Consent.Implementations
{
    public class RegisterTransactionService : IRegisterDataService<Transaction, IncomingTransactionModel>
    {
        private readonly ILogger _logger;
        private readonly IRepository<Transaction, IncomingTransactionModel> _repository;

        public RegisterTransactionService(
            IRepository<Transaction, IncomingTransactionModel> repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        private async Task RegisterData(IncomingTransactionModel model, DataSources dataSource)
        {
            try
            {
                model.DataSource = dataSource;
                model.TimeStamp = DateTime.UtcNow.ToUnixTimeMilliseconds();

                string id = model.Id ?? string.Empty;
                if (model is IModelWithOwnership ownedModel)
                    await _repository.Upsert(id, model, ownedModel.UserId);
                else
                    await _repository.Upsert(id, model);
            }

            catch (Exception e)
            {
                _logger.Fatal(e, "Unhandled exception for the data source {DataSource} with data packet {Model}",
                    dataSource, model);
            }
        }

        public Task Execute(string message, DataSources dataSource)
        {
            // Turn to object
            IncomingTransactionModel incomingTransactionModel = Json
                .Deserialize<IncomingTransactionModel>(message);

            return RegisterData(incomingTransactionModel, dataSource);
        }
    }
}