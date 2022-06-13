// using Airslip.Common.Auth.Interfaces;
// using Airslip.Common.Auth.Models;
// using Airslip.Common.Repository.Types.Interfaces;
// using Airslip.Common.Repository.Types.Models;
// using Airslip.Common.Services.Consent.Entities;
// using Airslip.Common.Services.Consent.Interfaces;
// using Airslip.Common.Services.Consent.Models;
// using Airslip.Common.Types.Interfaces;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
//
// namespace Airslip.Common.Services.Consent.Implementations
// {
//     public class AccountService : IAccountService
//     {
//         private readonly IRepository<Account, AccountResponse> _repository;
//         private readonly IEntitySearch<Account, AccountResponse> _entitySearch;
//         private readonly UserToken _userToken;
//
//         public AccountService(
//             IRepository<Account, AccountResponse> repository,
//             IEntitySearch<Account, AccountResponse> entitySearch,
//             ITokenDecodeService<UserToken> tokenDecodeService)
//         {
//             _repository = repository;
//             _entitySearch = entitySearch;
//             _userToken = tokenDecodeService.GetCurrentToken();
//         }
//         
//         public async Task<IResponse> GetAccounts()
//         {
//             List<AccountResponse> accounts = await _entitySearch
//                 .GetSearchResults(new List<SearchFilterModel>()
//             {
//                 new(nameof(Account.UserId), _userToken.UserId)
//             });
//             
//             return new AccountsResponse(accounts);
//         }
//
//         public async Task<IResponse> GetAccount(string accountId)
//         {
//             RepositoryActionResultModel<AccountResponse> resp = await _repository.Get(accountId);
//             
//             
//             List<AccountResponse> accounts = await _entitySearch
//                 .GetSearchResults(new List<SearchFilterModel>()
//                 {
//                     new(nameof(Account.Id), accountId),
//                     new(nameof(Account.UserId), _userToken.UserId)
//                 });
//             
//             return accounts.First();
//         }
//     }
// }