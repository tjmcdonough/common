using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Implementations.Events.Entity.PreProcess;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.UnitTests.Common;
using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Airslip.Common.Repository.UnitTests.Lifecycle.Events.Updates;

public class EntityOwnershipTests
{
    [Theory]
    [InlineData(LifecycleStage.Create, "my-entity-id", "my-user-id", AirslipUserType.Merchant)]
    [InlineData(LifecycleStage.Create, null, "my-user-id", AirslipUserType.Standard)]
    [InlineData(LifecycleStage.Create, "my-entity-id", "my-user-id", AirslipUserType.Standard)]
    [InlineData(LifecycleStage.Update, "my-entity-id", "my-user-id", AirslipUserType.Merchant)]
    [InlineData(LifecycleStage.Update, null, null, null)]
    [InlineData(LifecycleStage.Get, "my-entity-id", "my-user-id", AirslipUserType.Merchant)]
    [InlineData(LifecycleStage.Get, null, null, null)]
    [InlineData(LifecycleStage.Delete, "my-entity-id", "my-user-id", AirslipUserType.Merchant)]
    [InlineData(LifecycleStage.Delete, null, null, null)]
    public void Update_acts_as_expected(
        LifecycleStage lifecycleStage,
        string? entityId,
        string? userId,
        AirslipUserType? airslipUserType)
    {
        Mock<IUserContext> userContext = new();
        userContext.Setup(o => o.EntityId).Returns(entityId);
        userContext.Setup(o => o.UserId).Returns(userId);
        userContext.Setup(o => o.AirslipUserType).Returns(airslipUserType);

        IEntityPreProcessEvent<MyEntity> preProcessEvent =
            new EntityOwnershipEvent<MyEntity>(userContext.Object);

        MyEntity updatedEntity = preProcessEvent
            .Execute(new MyEntity(), lifecycleStage);
        
        switch (lifecycleStage)
        {
            case LifecycleStage.Create:
                updatedEntity.UserId.Should().Be(userId);
                updatedEntity.AirslipUserType.Should().Be(airslipUserType);

                if (airslipUserType == AirslipUserType.Standard)
                {
                    // Even if we provide an entity - they aren't relevant for standard users
                    if (entityId != null) updatedEntity.EntityId.Should().BeNull();
                }
                else
                {
                    updatedEntity.EntityId.Should().Be(entityId);
                }

                break;
            default:
                updatedEntity.EntityId.Should().BeNull();
                updatedEntity.UserId.Should().BeNull();
                updatedEntity.AirslipUserType.Should().Be(AirslipUserType.Unknown);
                break;
        }
    }
}