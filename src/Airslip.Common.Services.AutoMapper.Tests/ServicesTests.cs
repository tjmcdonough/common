// using Airslip.Common.Repository.Types.Entities;
// using Airslip.Common.Repository.Types.Enums;
// using Airslip.Common.Repository.Types.Interfaces;
// using Airslip.Common.Services.AutoMapper.Extensions;
// using Airslip.Common.Types.Enums;
// using Airslip.Common.Utilities;
// using FluentAssertions;
// using Microsoft.Extensions.DependencyInjection;
// using System;
// using Xunit;
//
// namespace Airslip.Common.Services.AutoMapper.Tests
// {
//     public class 
//         ServicesTests
//     {
//         [Fact]
//         public void Mapping_model_with_ownership_doesnt_update_entity()
//         {
//             IServiceCollection services = new ServiceCollection();
//             services.AddAutoMapper(cfg =>
//             {
//                 cfg.CreateMap<MyEntity, MyModel>().ReverseMap();
//             });
//
//             ServiceProvider provider = services.BuildServiceProvider() ?? 
//                                         throw new NotImplementedException();
//
//             IModelMapper<MyModel> mapper = provider.GetService<IModelMapper<MyModel>>() ?? 
//                                            throw new NotImplementedException();
//
//             MyModel myModel = new()
//             {
//                 Id = CommonFunctions.GetId(),
//                 EntityId = CommonFunctions.GetId(),
//                 AirslipUserType = AirslipUserType.Merchant,
//                 EntityStatus = EntityStatus.Active
//             };
//
//             MyEntity result = mapper.Create<MyEntity>(myModel);
//
//             result.EntityId.Should().BeNull();
//             result.AirslipUserType.Should().Be(AirslipUserType.Unknown);
//         }
//         [Fact]
//         public void Mapping_entity_with_ownership_does_update_model()
//         {
//             IServiceCollection services = new ServiceCollection();
//             services.AddAutoMapper(cfg =>
//             {
//                 cfg.CreateMap<MyEntity, MyModel>().ReverseMap();
//             });
//
//             ServiceProvider provider = services.BuildServiceProvider() ?? 
//                                        throw new NotImplementedException();
//
//             IModelMapper<MyEntity> mapper = provider.GetService<IModelMapper<MyEntity>>() ?? 
//                                            throw new NotImplementedException();
//
//             MyEntity myEntity = new()
//             {
//                 Id = CommonFunctions.GetId(),
//                 EntityId = CommonFunctions.GetId(),
//                 AirslipUserType = AirslipUserType.Merchant,
//                 EntityStatus = EntityStatus.Active
//             };
//
//             MyModel result = mapper.Create<MyModel>(myEntity);
//
//             result.EntityId.Should().Be(myEntity.EntityId);
//             result.AirslipUserType.Should().Be(myEntity.AirslipUserType);
//         }
//
//         public class MyEntity : IEntityWithOwnership
//         {
//             public string Id { get; set; } = string.Empty;
//             public BasicAuditInformation? AuditInformation { get; set; }
//             public EntityStatus EntityStatus { get; set; }
//             public string? UserId { get; set; }
//             public string? EntityId { get; set; }
//             public AirslipUserType AirslipUserType { get; set; }
//         }
//         
//         public class MyModel : IModelWithOwnership
//         {
//             public string? Id { get; set; }
//             public EntityStatus EntityStatus { get; set; }
//             public string? UserId { get; set; }
//             public string? EntityId { get; set; }
//             public AirslipUserType AirslipUserType { get; set; }
//         }
//     }
// }