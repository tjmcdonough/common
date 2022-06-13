// using Airslip.Common.Repository.Types.Interfaces;
// using Airslip.Common.Repository.Types.Models;
// using Airslip.Common.Services.Consent.Entities;
// using Airslip.Common.Services.Consent.Interfaces;
// using Airslip.Common.Services.Consent.Models;
// using Airslip.Common.Types.Interfaces;
// using System.Collections.Generic;
// using System.Threading.Tasks;
//
// namespace Airslip.Common.Services.Consent.Implementations
// {
//     public class BankService : IBankService
//     {
//         private readonly IEntitySearch<Bank, BankResponse> _entitySearch;
//
//         public BankService(IEntitySearch<Bank, BankResponse> entitySearch)
//         {
//             _entitySearch = entitySearch;
//         }
//         
//         public async Task<IResponse> GetBanks(string countryCode, string? name)
//         {
//             List<BankResponse> accounts = await _entitySearch
//                 .GetSearchResults(new List<SearchFilterModel>());
//             
//             return new BanksResponse(accounts);
//         }
//     }
// }