using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Implementations;
using Airslip.Common.Repository.Implementations.Events.Entity.PreProcess;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.UnitTests.Common;
using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace Airslip.Common.Repository.UnitTests.Lifecycle.Events.Updates;

public class EntityTimeStampTests
{
    private const long withTimeStamp = 123456789;
    private readonly Mock<IDateTimeProvider> dateTimeProvider;
    public EntityTimeStampTests()
    {
        dateTimeProvider = new Mock<IDateTimeProvider>();
        dateTimeProvider.Setup(o => o.GetCurrentUnixTime()).Returns(withTimeStamp);
        dateTimeProvider.Setup(o => o.GetUtcNow()).Returns(DateTime.UtcNow);
    }
    
    [Theory]
    [InlineData(LifecycleStage.Update)]
    [InlineData(LifecycleStage.Create)]
    [InlineData(LifecycleStage.Delete)]
    public void Update_acts_as_expected(
        LifecycleStage lifecycleStage)
    {
        IEntityPreProcessEvent<MyEntityWithTimeStamp> preProcessEvent =
            new EntityTimeStampEvent<MyEntityWithTimeStamp>(dateTimeProvider.Object);
            
        MyEntityWithTimeStamp updatedEntity = preProcessEvent
            .Execute(new MyEntityWithTimeStamp()
            {
                TimeStamp = 5
            }, lifecycleStage);

        updatedEntity.TimeStamp.Should().Be(withTimeStamp);
    }
    
    [Theory]
    [InlineData(LifecycleStage.Update)]
    [InlineData(LifecycleStage.Create)]
    [InlineData(LifecycleStage.Delete)]
    public void Update_acts_as_expected_when_interface_not_implemented(
        LifecycleStage lifecycleStage)
    {
        IEntityPreProcessEvent<MyEntity> preProcessEvent =
            new EntityTimeStampEvent<MyEntity>(dateTimeProvider.Object);
            
        MyEntity updatedEntity = preProcessEvent
            .Execute(new MyEntity()
            {
                TimeStamp = 5
            }, lifecycleStage);

        updatedEntity.TimeStamp.Should().Be(5);
    }
}