using Airslip.Common.Repository.Types.Interfaces;
using System.Text.Json.Serialization;

namespace Airslip.Common.CustomerPortal.Interfaces
{
    public interface ICustomerAccountModel : IModelWithOwnership
    {
        /// <summary>
        /// The Airslip API Key value
        /// </summary>
        [JsonIgnore]
        public string InternalApiKey { get; set; }
        /// <summary>
        /// The name of account or store name
        /// </summary>
        public string? Name { get; set; }
    }
}