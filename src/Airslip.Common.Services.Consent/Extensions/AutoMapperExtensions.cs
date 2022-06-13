using Airslip.Common.Matching.Data;
using Airslip.Common.Services.Consent.Entities;
using Airslip.Common.Services.Consent.Implementations;
using Airslip.Common.Services.Consent.Models;
using Airslip.Common.Types;
using Airslip.Common.Utilities.Extensions;
using AutoMapper;
using System.Globalization;

namespace Airslip.Common.Services.Consent.Extensions
{
    public static class AutoMapperExtensions
    {
        public static void AddTransactionMapperConfiguration(this IMapperConfigurationExpression mapperConfigurationExpression)
        {
            mapperConfigurationExpression
                .CreateMap<IncomingTransactionModel, Transaction>()
                .ForPath(d => d.Amount, c => c
                    .MapFrom(s => Currency.ConvertToUnit(s.Amount)))
                .ForPath(d => d.Description, c => c
                    .MapFrom(s => s.Description))
                .ForPath(d => d.CurrencyCode, c => c
                    .MapFrom(s => s.CurrencyCode))
                .ForPath(d => d.MatchType, c => c
                    .MapFrom(src => MatchTypes.Yapily))
                .ForPath(d => d.UserId, c => c
                    .MapFrom(s => s.UserId))
                .ForPath(d => d.AuthorisedTimeStamp, c => c
                    .MapFrom(s => s.AuthorisedDate))
                .ForPath(d => d.CapturedTimeStamp, c => c
                    .MapFrom(s => s.CapturedDate))
                .ForPath(d => d.Id, c => c
                    .MapFrom(s => s.Id))
                .ForPath(d => d.DataSource, c => c
                    .MapFrom(s =>s.DataSource))
                .ForPath(d => d.Merchant, c => c.Ignore())
                .ForPath(d => d.Metadata, c => c.Ignore())
                .ReverseMap();

            mapperConfigurationExpression
                .CreateMap<IncomingTransactionModel, TransactionBank>()
                .ForPath(d => d.AccountId, c => c
                    .MapFrom(s => s.AccountId))
                .ForPath(d => d.BankIcon, c => c
                    .Ignore())
                .ForPath(d => d.BankName, c => c
                    .Ignore())
                .ForPath(d => d.BankTransactionId, c => c
                    .MapFrom(s => s.BankTransactionId))
                .ReverseMap();

            mapperConfigurationExpression
                .CreateMap<IncomingTransactionModel, TransactionMerchant>()
                .ForPath(d => d.EntityId, c => c
                    .MapFrom(s => s.EntityId))
                .ReverseMap();
        }

        public static void AddConsentMapperConfiguration(this IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Account, AccountModel>()
                .ReverseMap();
            cfg.CreateMap<Bank, BankModel>()
                .ReverseMap();
            cfg.CreateMap<Transaction, TransactionModel>()
                .ForMember(s => s.CapturedDate, c => c.MapFrom(d => d.CapturedTimeStamp!.Value.ToUtcDate().ToString(CultureInfo.InvariantCulture)))
                .ForMember(s => s.CapturedTime, c => c.MapFrom(d => d.CapturedTimeStamp!.Value.GetTime().ToString(CultureInfo.InvariantCulture)))
                .ForMember(s => s.AuthorisedDate, c => c.MapFrom(d => d.AuthorisedTimeStamp != null ? d.AuthorisedTimeStamp.Value.ToUtcDate().ToString(CultureInfo.InvariantCulture) : null))
                .ForMember(s => s.AuthorisedTime, c => c.MapFrom(d => d.AuthorisedTimeStamp != null ? d.AuthorisedTimeStamp.Value.GetTime().ToString(CultureInfo.InvariantCulture) : null))
                .ForMember(s => s.CurrencySymbol, c => c.MapFrom(d => Currency.GetSymbol(d.CurrencyCode!)))
                .ForMember(s => s.Amount, c => c.MapFrom(d =>  Currency.ConvertToTwoPlacedDecimal(d.Amount).ToString()));
            cfg.CreateMap<Transaction, TransactionSummaryModel>()
                .ForMember(s => s.AccountId, c => c.MapFrom(d => d.BankDetails.AccountId))
                .ForMember(s => s.TimeStamp, c => c.MapFrom(d => d.CapturedTimeStamp ?? d.TimeStamp));
            cfg.CreateMap<TransactionBank, TransactionBankModel>();
            cfg.CreateMap<TransactionMerchant, TransactionMerchantModel>().ReverseMap();
            cfg.CreateMap<TransactionMerchant, MerchantSummaryModel>()
                .ForMember(s => s.Id, c => c.MapFrom(d => d.EntityId));
            cfg.CreateMap<Merchant, MerchantSummaryModel>().ReverseMap();
            cfg.CreateMap<Account, AccountResponse>();
            cfg.CreateMap<Bank, BankResponse>();
        }
    }
}