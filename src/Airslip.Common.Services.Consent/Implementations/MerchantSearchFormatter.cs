using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Services.Consent.Entities;
using Airslip.Common.Services.Consent.Interfaces;
using Airslip.Common.Services.Consent.Models;
using Airslip.Common.Types.Configuration;
using Airslip.Common.Utilities.Extensions;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Airslip.Common.Services.Consent.Implementations
{
    public class MerchantSearchFormatter<TModel> : IEntitySearchFormatter<TModel>
        where TModel : class, IMerchantDetails, IModel
    {
        private readonly IContext _context;
        private readonly IModelMapper<MerchantSummaryModel> _mapper;
        private readonly string _baseUri;

        public MerchantSearchFormatter(IContext context, IModelMapper<MerchantSummaryModel> mapper,
            IOptions<PublicApiSettings> options)
        {
            _context = context;
            _mapper = mapper;
            _baseUri = options.Value.GetSettingByName("CustomerPortalApi").ToBaseUri();
        }
        
        public async Task<TModel> FormatModel(TModel model)
        {
            // Populate with merchant...
            if (string.IsNullOrWhiteSpace(model.MerchantDetails.Id))
            {
                return model;
            }

            Merchant? merchant = await _context
                .GetEntity<Merchant>(model.MerchantDetails.Id);

            if (merchant == null)
            {
                return model;
            }
            
            model.MerchantDetails = _mapper.Create(merchant);
            if (model.Id == null) return model;
            
            model.MerchantDetails.LogoUrl = TransactionModel.Endpoints.GetLogoUrl(_baseUri, model.Id);
            model.MerchantDetails.IconUrl = TransactionModel.Endpoints.GetIconUrl(_baseUri, model.Id);

            return model;
        }
    }
}