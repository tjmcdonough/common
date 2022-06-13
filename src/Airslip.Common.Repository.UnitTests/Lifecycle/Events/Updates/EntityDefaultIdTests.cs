using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Implementations.Events.Entity.PreProcess;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.UnitTests.Common;
using FluentAssertions;
using Xunit;

namespace Airslip.Common.Repository.UnitTests.Lifecycle.Events.Updates;

public class EntityDefaultIdTests
{
    [Theory]
    [InlineData(LifecycleStage.Update, "")]
    [InlineData(LifecycleStage.Create, "predefined-id")]
    [InlineData(LifecycleStage.Create, "")]
    [InlineData(LifecycleStage.Delete, "")]
    [InlineData(LifecycleStage.Get, "")]
    public void Update_acts_as_expected(
        LifecycleStage lifecycleStage,
        string withId)
    {
        IEntityPreProcessEvent<MyEntity> preProcessEvent =
            new EntityDefaultIdEvent<MyEntity>();
            
        MyEntity updatedEntity = preProcessEvent
            .Execute(new MyEntity
            {
                Id = withId
            }, lifecycleStage);
        
        switch (lifecycleStage)
        {
            case LifecycleStage.Create:
                updatedEntity.Id.Should().NotBeNullOrWhiteSpace();
                if (!withId.Equals(string.Empty)) updatedEntity.Id.Should().Be(withId);
                if (withId.Equals(string.Empty)) updatedEntity.Id.Should().NotBe(withId);
                break;
            default:
                updatedEntity.Id.Should().Be(string.Empty);
                break;
        }
    }
}