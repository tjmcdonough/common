using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Implementations.Events.Entity.PreProcess;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Repository.Types.Models;
using Airslip.Common.Repository.UnitTests.Common;
using Airslip.Common.Types.Enums;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Airslip.Common.Repository.UnitTests;

public class RepositoryFunctionTests
{
    [Fact]
    public async Task Can_upsert_new_record()
    {
        IServiceProvider provider = Helpers.BuildRepoProvider();

        IRepository<MyEntity, MyModel> repo = provider.GetService<IRepository<MyEntity, MyModel>>()
                                              ?? throw new NotImplementedException();

        RepositoryActionResultModel<MyModel> delete = await repo.Upsert("unknown-id", new MyModel
        {
            Id = "unknown-id"
        });

        delete.Should().BeOfType<SuccessfulActionResultModel<MyModel>>();
        delete.ResultType.Should().Be(ResultType.Success);
    }
        
    [Fact]
    public void Ownership_sets_correctly_when_data_in_entity()
    {
        IServiceProvider provider = Helpers.BuildRepoProvider("notMyEntityId", "notMyUserId");

        IEnumerable<IEntityPreProcessEvent<MyEntity>> preProcessors = provider
                                                                          .GetService<IEnumerable<IEntityPreProcessEvent<MyEntity>>>()
                                                                      ?? throw new NotImplementedException();

        IEntityPreProcessEvent<MyEntity> preProcessor = preProcessors
            .First(o => o is EntityOwnershipEvent<MyEntity>);
                        
        MyEntity myEntity = preProcessor.Execute(new MyEntity
        {
            AirslipUserType = AirslipUserType.Merchant,
            EntityId = "myEntityId",
            UserId = "myUserId"
        }, LifecycleStage.Create);

        myEntity.AirslipUserType.Should().Be(AirslipUserType.Merchant);
        myEntity.EntityId.Should().Be("myEntityId");
        myEntity.UserId.Should().Be("myUserId");
    }
        
    [Fact]
    public void Ownership_sets_correctly_when_no_data_in_entity()
    {
        IServiceProvider provider = Helpers.BuildRepoProvider("myEntityId", "myUserId");

        IEnumerable<IEntityPreProcessEvent<MyEntity>> preProcessors = provider
                                                                          .GetService<IEnumerable<IEntityPreProcessEvent<MyEntity>>>()
                                                                      ?? throw new NotImplementedException();

        IEntityPreProcessEvent<MyEntity> preProcessor = preProcessors
            .First(o => o is EntityOwnershipEvent<MyEntity>);
                        
        MyEntity myEntity = preProcessor.Execute(new MyEntity(), LifecycleStage.Create);

        myEntity.AirslipUserType.Should().Be(AirslipUserType.InternalApi);
        myEntity.EntityId.Should().Be("myEntityId");
        myEntity.UserId.Should().Be("myUserId");
    }
}