using Airslip.Common.MerchantTransactions.Generated;
using Airslip.Common.MerchantTransactions.Interfaces;
using Airslip.Common.Types.Enums;
using Airslip.Common.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airslip.Common.MerchantTransactions.Implementations
{
    public class MerchantIntegrationService<TSource> : IMerchantIntegrationService<TSource> 
        where TSource : class
    {
        private readonly IInternalApiV1Client _internalApiV1Client;
        private readonly IExternalApiV1Client _externalApiV1Client;
        private readonly ITransactionMapper<TSource> _transactionMapper;

        public MerchantIntegrationService(IInternalApiV1Client internalApiV1Client, 
            IExternalApiV1Client externalApiV1Client,
            ITransactionMapper<TSource> transactionMapper)
        {
            _internalApiV1Client = internalApiV1Client;
            _externalApiV1Client = externalApiV1Client;
            _transactionMapper = transactionMapper;
        }

        public async Task<ICollection<TrackingDetails>> SendBulk(
            IEnumerable<TSource> transactions,
            string accountId,
            string entityId,
            AirslipUserType airslipUserType,
            string userId, 
            PosProviders provider)
        {
            List<TrackingDetails> trackingDetails = new();

            foreach (TSource transaction in transactions)
                trackingDetails.Add(await _sendInternalApi(transaction, accountId, entityId, airslipUserType, userId, provider));

            return trackingDetails;
        }

        public Task<TrackingDetails> Send(TSource transaction,
            string accountId,
            string entityId,
            AirslipUserType airslipUserType,
            string userId, 
            PosProviders provider)
        {
            return _sendInternalApi(transaction, accountId, entityId, airslipUserType, userId, provider);
        }

        public async Task<ICollection<TrackingDetails>> SendBulk(IEnumerable<TSource> transactions, 
            string airslipApiKey, PosProviders provider)
        {
            List<TrackingDetails> trackingDetails = new();

            foreach (TSource transaction in transactions)
                trackingDetails.Add(await _sendExternalApi(transaction, airslipApiKey, provider));

            return trackingDetails;
        }

        public Task<TrackingDetails> Send(TSource transaction, string airslipApiKey, PosProviders provider)
        {
            return _sendExternalApi(transaction, airslipApiKey, provider);
        }

        private Task<TrackingDetails> _sendInternalApi(TSource transaction, 
            string accountId,
            string entityId,
            AirslipUserType airslipUserType,
            string userId, 
            PosProviders provider)
        {
            TransactionDetails transactionOut = _transactionMapper
                .Create(transaction, provider);
                
            transactionOut.InternalId = CommonFunctions.GetId();
            transactionOut.Source = provider.ToString();

            return _internalApiV1Client
                .CreateTransactionAsync(accountId, entityId, airslipUserType.ToString(), userId, transactionOut);
        }
        
        private Task<TrackingDetails> _sendExternalApi(TSource transaction, string apiKeyToken, 
            PosProviders provider)
        {
            TransactionDetails transactionOut = _transactionMapper.Create(
                transaction, provider);
                
            transactionOut.InternalId = CommonFunctions.GetId();
            transactionOut.Source = provider.ToString();

            _externalApiV1Client.SetApiKeyToken(apiKeyToken);
            return _externalApiV1Client.CreateTransactionAsync(transactionOut);
        }
    }
}