using Airslip.Common.Repository.Data;
using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Implementations;
using Airslip.Common.Repository.Implementations.Events.Entity.PreProcess;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.UnitTests.Common;
using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Airslip.Common.Repository.UnitTests.Lifecycle.Events.Updates;

public class EntityBasicAuditTests
{
    [Theory]
    [InlineData(LifecycleStage.Update)]
    [InlineData(LifecycleStage.Create)]
    [InlineData(LifecycleStage.Delete)]
    [InlineData(LifecycleStage.Get)]
    public void Update_acts_as_expected(
        LifecycleStage lifecycleStage)
    {
        Mock<IUserContext> userContext = new();
        userContext.Setup(o => o.EntityId).Returns((string?)null);
        userContext.Setup(o => o.UserId).Returns("my-user-id");
        userContext.Setup(o => o.AirslipUserType).Returns(AirslipUserType.Standard);

        IEntityPreProcessEvent<MyEntity> preProcessEvent =
            new EntityBasicAuditEvent<MyEntity>(userContext.Object, new DateTimeProvider());
            
        RepositoryAction<MyEntity, MyModel> repositoryAction 
            = new(null, new MyEntity(), null, lifecycleStage, null);

        MyEntity updatedEntity = preProcessEvent
            .Execute(repositoryAction.Entity!, lifecycleStage);

        
        switch (lifecycleStage)
        {
            case LifecycleStage.Update:
                updatedEntity.AuditInformation.Should().NotBeNull();
                updatedEntity.AuditInformation!.CreatedByUserId.Should().Be(userContext.Object.UserId);
                updatedEntity.AuditInformation.UpdatedByUserId.Should().Be(userContext.Object.UserId);
                updatedEntity.AuditInformation.DateUpdated.Should().NotBeNull();
                break;
            case LifecycleStage.Create:
                updatedEntity.AuditInformation.Should().NotBeNull();
                updatedEntity.AuditInformation!.CreatedByUserId.Should().Be(userContext.Object.UserId);
                break;
            case LifecycleStage.Delete:
                updatedEntity.AuditInformation.Should().NotBeNull();
                updatedEntity.AuditInformation!.DeletedByUserId.Should().Be(userContext.Object.UserId);
                updatedEntity.AuditInformation.DateDeleted.Should().NotBeNull();
                break;
            case LifecycleStage.Get:
                updatedEntity.AuditInformation.Should().BeNull();
                break;
        }
    }
}