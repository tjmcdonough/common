namespace Airslip.Common.Services.Consent.Models
{
    public record TransactionMerchantModel
    {
        public TransactionMerchantModel()
        {
            
        }
        
        public TransactionMerchantModel(string entityId)
        {
            EntityId = entityId;
        }

        public string? EntityId { get; set; }
    }
}