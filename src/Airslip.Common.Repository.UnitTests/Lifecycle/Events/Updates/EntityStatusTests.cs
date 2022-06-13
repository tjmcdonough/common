using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Implementations.Events.Entity.PreProcess;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Repository.UnitTests.Common;
using FluentAssertions;
using Xunit;

namespace Airslip.Common.Repository.UnitTests.Lifecycle.Events.Updates;

public class EntityStatusTests
{
    [Theory]
    [InlineData(LifecycleStage.Update, EntityStatus.Active)]
    [InlineData(LifecycleStage.Create, EntityStatus.Active)]
    [InlineData(LifecycleStage.Delete, EntityStatus.Deleted)]
    public void Update_acts_as_expected(
        LifecycleStage lifecycleStage,
        EntityStatus entityStatus)
    {
        IEntityPreProcessEvent<MyEntity> preProcessEvent =
            new EntityStatusEvent<MyEntity>();
            
        MyEntity updatedEntity = preProcessEvent
            .Execute(new MyEntity(), lifecycleStage);

        updatedEntity.EntityStatus.Should().Be(entityStatus);
    }
}