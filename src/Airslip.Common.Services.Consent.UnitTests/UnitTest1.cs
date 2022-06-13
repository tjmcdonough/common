using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Services.AutoMapper;
using Airslip.Common.Services.AutoMapper.Extensions;
using Airslip.Common.Services.Consent.Data;
using Airslip.Common.Services.Consent.Entities;
using Airslip.Common.Services.Consent.Enums;
using Airslip.Common.Services.Consent.Extensions;
using Airslip.Common.Services.Consent.Models;
using Airslip.Common.Types.Configuration;
using Airslip.Common.Types.Enums;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Airslip.Common.Services.Consent.UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void Account_model_maps_as_expected()
        {
            ConfigurationBuilder builder = new();
            IConfigurationRoot configuration= builder.Build();
            IServiceCollection services = new ServiceCollection();
            services.AddConsentAuthorisation(configuration);
            services.AddAutoMapper(cfg =>
            {
                cfg.AddTransactionMapperConfiguration();
                cfg.AddConsentMapperConfiguration();
            }, MapperUsageType.Service);

            ServiceProvider provider = services.BuildServiceProvider();

            using IServiceScope scope = provider.CreateScope();
            
            IModelMapper<AccountModel> mappers = scope
                .ServiceProvider
                .GetService<IModelMapper<AccountModel>>() ?? throw new NotImplementedException();

            AccountModel model = new()
            {
                Id = "SomeId",
                AccountId = "AccountId",
                AccountNumber = "AccountNumber",
                AccountStatus = AccountStatus.Active,
                AccountType = "AccountType",
                CurrencyCode = "CurrencyCode",
                DataSource = DataSources.Yapily,
                EntityId = "EntityId",
                EntityStatus = EntityStatus.Active,
                InstitutionId = "InstitutionId",
                SortCode = "SortCode",
                TimeStamp = 1,
                UsageType = "UsageType",
                UserId = "UserId",
                AirslipUserType = AirslipUserType.Merchant,
                CreatedTimeStamp = 2,
                LastCardDigits = "LastCardDigits"
            };

            Account entity = mappers
                .Create<Account>(model);

            entity.Id.Should().Be(model.Id);
            entity.AccountId.Should().Be(model.AccountId);
            entity.AccountNumber.Should().Be(model.AccountNumber);
            entity.AccountStatus.Should().Be(model.AccountStatus);
            entity.AccountType.Should().Be(model.AccountType);
            entity.CurrencyCode.Should().Be(model.CurrencyCode);
            entity.DataSource.Should().Be(model.DataSource);
            entity.EntityId.Should().Be(model.EntityId);
            entity.EntityStatus.Should().Be(model.EntityStatus);
            entity.InstitutionId.Should().Be(model.InstitutionId);
            entity.SortCode.Should().Be(model.SortCode);
            entity.TimeStamp.Should().Be(model.TimeStamp);
            entity.UsageType.Should().Be(model.UsageType);
            entity.UserId.Should().Be(model.UserId);
            entity.AirslipUserType.Should().Be(model.AirslipUserType);
            // entity.CreatedTimeStamp = 2,
            entity.LastCardDigits.Should().Be(model.LastCardDigits);
        }
        
        [Theory]
        [InlineData("some-merchant")]
        [InlineData(null)]
        public void AccountId_maps_as_expected_when_null(string? accountId)
        {
            ConfigurationBuilder builder = new();
            IConfigurationRoot configuration= builder.Build();
            IServiceCollection services = new ServiceCollection();
            services.AddConsentAuthorisation(configuration);
            services.AddAutoMapper(cfg =>
            {
                cfg.AddTransactionMapperConfiguration();
                cfg.AddConsentMapperConfiguration();
            }, MapperUsageType.Service);

            ServiceProvider provider = services.BuildServiceProvider();

            using IServiceScope scope = provider.CreateScope();
            
            IModelMapper<TransactionSummaryModel> mappers = scope
                .ServiceProvider
                .GetService<IModelMapper<TransactionSummaryModel>>() ?? throw new NotImplementedException();

            Transaction entity = new()
            {
                BankDetails = new TransactionBank()
                {
                    AccountId = accountId
                }
            };

            TransactionSummaryModel model = mappers
                .Create(entity);

            model.AccountId.Should().Be(accountId);
        }
        
        [Fact]
        public async Task Merchant_maps_as_expected_unknown_merchant()
        {
            ConfigurationBuilder builder = new();
            IConfigurationRoot configuration= builder.Build();
            IServiceCollection services = new ServiceCollection();
            Mock<IContext> context = new();
            PublicApiSettings settings = new()
            {
                Settings = new Dictionary<string, PublicApiSetting>
                {
                    {"CustomerPortalApi", new PublicApiSetting()}
                }
            };
            services.AddSingleton(Options.Create(settings));
            services.AddConsentAuthorisation(configuration);
            
            services.AddAutoMapper(cfg =>
            {
                cfg.AddTransactionMapperConfiguration();
                cfg.AddConsentMapperConfiguration();
            }, MapperUsageType.Service);
            services.AddSingleton(context.Object);
            
            ServiceProvider provider = services.BuildServiceProvider();

            using IServiceScope scope = provider.CreateScope();
            
            IEntitySearchFormatter<TransactionSummaryModel> mappers = scope
                .ServiceProvider
                .GetService<IEntitySearchFormatter<TransactionSummaryModel>>() ?? throw new NotImplementedException();

            TransactionSummaryModel model = new()
            {
                Id = "SomeId",
                CurrencyCode = "CurrencyCode",
                EntityStatus = EntityStatus.Active,
                MerchantDetails = new MerchantSummaryModel
                {
                    Id = null
                }
            };

            TransactionSummaryModel entity = await mappers
                .FormatModel(model);

            entity.Id.Should().Be(model.Id);
            entity.MerchantDetails.Should().NotBeNull();
            entity.MerchantDetails.CategoryCode.Should().Be(Constants.DEFAULT_CATEGORY_CODE);
        }
        
        [Theory]
        [InlineData("some-merchant", "My merchant name 1", MerchantTypes.Unsupported, "cat1", "cat1")]
        [InlineData("some-merchant","My merchant name 2", MerchantTypes.Customer, "cat2", "cat2")]
        [InlineData("some-merchant","My merchant name 3", MerchantTypes.Merchant, "cat4", "cat4")]
        [InlineData("some-merchant","My merchant name 4", MerchantTypes.PaymentProcessor, "cat6", "cat6")]
        [InlineData(null, null, MerchantTypes.Unsupported, null, Constants.DEFAULT_CATEGORY_CODE)]
        public async Task Merchant_maps_as_expected_with_data(string? merchantId, string? merchantName, MerchantTypes merchantType,
            string? categoryCode, string expectedCatCode)
        {
            Merchant merchant = new(merchantName ?? string.Empty, merchantType)
            {
                Id = merchantId ?? string.Empty,
                CategoryCode = categoryCode
            };
            
            ConfigurationBuilder builder = new();
            IConfigurationRoot configuration= builder.Build();
            IServiceCollection services = new ServiceCollection();
            Mock<IContext> context = new();

            context
                .Setup(o => o.GetEntity<Merchant>(merchantId ?? string.Empty))
                .Returns(async () => await Task.FromResult(merchant));
            
            PublicApiSettings settings = new()
            {
                Settings = new Dictionary<string, PublicApiSetting>
                {
                    {"CustomerPortalApi", new PublicApiSetting()}
                }
            };
            services.AddSingleton(Options.Create(settings));
            services.AddConsentAuthorisation(configuration);
            
            services.AddAutoMapper(cfg =>
            {
                cfg.AddTransactionMapperConfiguration();
                cfg.AddConsentMapperConfiguration();
            }, MapperUsageType.Service);
            services.AddSingleton(context.Object);
            
            ServiceProvider provider = services.BuildServiceProvider();

            using IServiceScope scope = provider.CreateScope();
            
            IEntitySearchFormatter<TransactionSummaryModel> mappers = scope
                .ServiceProvider
                .GetService<IEntitySearchFormatter<TransactionSummaryModel>>() ?? throw new NotImplementedException();

            TransactionSummaryModel model = new()
            {
                Id = "SomeId",
                CurrencyCode = "CurrencyCode",
                EntityStatus = EntityStatus.Active,
                MerchantDetails = new MerchantSummaryModel
                {
                    Id = merchantId
                }
            };

            TransactionSummaryModel entity = await mappers
                .FormatModel(model);

            entity.Id.Should().Be(model.Id);
            entity.MerchantDetails.Should().NotBeNull();
            entity.MerchantDetails.Id.Should().Be(merchantId);
            entity.MerchantDetails.CategoryCode.Should().Be(expectedCatCode);
            entity.MerchantDetails.Name.Should().Be(merchantName);
            entity.MerchantDetails.Type.Should().Be(merchantType);
        }
    }
}