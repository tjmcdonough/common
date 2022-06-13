using Airslip.Common.Repository.Data;
using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Implementations.Events.Entity.PreValidate;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Types.Entities;
using Airslip.Common.Repository.Types.Models;
using Airslip.Common.Repository.UnitTests.Common;
using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Interfaces;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Airslip.Common.Repository.UnitTests.Lifecycle.Events.Validation;

public class EntityOwnershipValidationTests
{
    [Theory]
    [InlineData(LifecycleStage.Update, true, "my-entity-id", "my-user-id", AirslipUserType.Merchant, 0, null)]
    [InlineData(LifecycleStage.Update, true, "not-my-entity-id", "my-user-id", AirslipUserType.Merchant, 1, ErrorMessages.OwnershipCannotBeVerified)]
    [InlineData(LifecycleStage.Update, true, null, "my-user-id", AirslipUserType.Standard, 1, ErrorMessages.OwnershipCannotBeVerified)]
    [InlineData(LifecycleStage.Update, true, "my-entity-id", "not-my-user-id", AirslipUserType.Merchant, 0, null)]
    [InlineData(LifecycleStage.Create, true, "my-entity-id", "my-user-id", AirslipUserType.Merchant, 1, ErrorMessages.LifecycleEventDoesntApply)]
    [InlineData(LifecycleStage.Get, true, "my-entity-id", "my-user-id", AirslipUserType.Merchant, 0, null)]
    [InlineData(LifecycleStage.Delete, true, "my-entity-id", "my-user-id", AirslipUserType.Merchant, 0, null)]
    public async Task Validation_acts_as_expected_for_merchant(
        LifecycleStage lifecycleStage, 
        bool createEntity,
        string? entityId,
        string? userId,
        AirslipUserType airslipUserType,
        int resultCount,
        string? expectedMessage)
    {
        Mock<IUserContext> userContext = new();
        userContext.Setup(o => o.EntityId).Returns("my-entity-id");
        userContext.Setup(o => o.UserId).Returns("my-user-id");
        userContext.Setup(o => o.AirslipUserType).Returns(AirslipUserType.Merchant);
        
        IEntityPreValidateEvent<MyEntity, MyModel> validateEvent =
            new EntityOwnershipValidationEvent<MyEntity, MyModel>(userContext.Object);
            
        RepositoryAction<MyEntity, MyModel> repositoryAction 
            = new(null, createEntity ? new MyEntity
            {
                EntityId = entityId,
                UserId = userId,
                AirslipUserType = airslipUserType
            } : null, null, lifecycleStage, null);

        List<ValidationResultMessageModel> validationResult = await validateEvent
            .Validate(repositoryAction);

        validationResult.Count.Should().Be(resultCount);
        if (expectedMessage != null)
            validationResult.Count(o => o.Message.Equals(expectedMessage)).Should().Be(1);
    }
    
    [Theory]
    [InlineData(LifecycleStage.Update, true, "my-entity-id", "my-user-id", AirslipUserType.Merchant, 1, ErrorMessages.OwnershipCannotBeVerified)]
    [InlineData(LifecycleStage.Update, true, null, "my-user-id", AirslipUserType.Merchant, 1, ErrorMessages.OwnershipCannotBeVerified)]
    [InlineData(LifecycleStage.Update, true, null, "my-user-id", AirslipUserType.Standard, 0, null)]
    [InlineData(LifecycleStage.Create, true, null, "my-user-id", AirslipUserType.Standard, 1, ErrorMessages.LifecycleEventDoesntApply)]
    [InlineData(LifecycleStage.Get, true, null, "my-user-id", AirslipUserType.Standard, 0, null)]
    [InlineData(LifecycleStage.Delete, true, null, "my-user-id", AirslipUserType.Standard, 0, null)]
    public async Task Validation_acts_as_expected_for_standard(
        LifecycleStage lifecycleStage, 
        bool createEntity,
        string? entityId,
        string? userId,
        AirslipUserType airslipUserType,
        int resultCount,
        string? expectedMessage)
    {
        Mock<IUserContext> userContext = new();
        userContext.Setup(o => o.EntityId).Returns((string?)null);
        userContext.Setup(o => o.UserId).Returns("my-user-id");
        userContext.Setup(o => o.AirslipUserType).Returns(AirslipUserType.Standard);
        
        IEntityPreValidateEvent<MyEntity, MyModel> validateEvent =
            new EntityOwnershipValidationEvent<MyEntity, MyModel>(userContext.Object);
            
        RepositoryAction<MyEntity, MyModel> repositoryAction 
            = new(null, createEntity ? new MyEntity
            {
                EntityId = entityId,
                UserId = userId,
                AirslipUserType = airslipUserType
            } : null, null, lifecycleStage, null);

        List<ValidationResultMessageModel> validationResult = await validateEvent
            .Validate(repositoryAction);

        validationResult.Count.Should().Be(resultCount);
        if (expectedMessage != null)
            validationResult.Count(o => o.Message.Equals(expectedMessage)).Should().Be(1);
    }
    
    [Theory]
    [InlineData(LifecycleStage.Update, true, "my-entity-id", "my-user-id", AirslipUserType.Merchant, 0, null)]
    [InlineData(LifecycleStage.Get, true, "my-entity-id", "my-user-id", AirslipUserType.Merchant, 0, null)]
    [InlineData(LifecycleStage.Update, true, "not-my-entity-id", "not-my-user-id", AirslipUserType.Merchant, 1, ErrorMessages.OwnershipCannotBeVerified)]
    public async Task Validation_acts_as_expected_for_additional_owners(
        LifecycleStage lifecycleStage, 
        bool createEntity,
        string? entityId,
        string? userId,
        AirslipUserType airslipUserType,
        int resultCount,
        string? expectedMessage)
    {
        Mock<IUserContext> additionalUserContext = new();
        additionalUserContext.Setup(o => o.EntityId).Returns("my-entity-id");
        additionalUserContext.Setup(o => o.UserId).Returns("my-user-id");
        additionalUserContext.Setup(o => o.AirslipUserType).Returns(AirslipUserType.Merchant);
        
        IEntityPreValidateEvent<MyEntityWithAdditionalOwners, MyModel> validateEvent =
            new EntityOwnershipValidationEvent<MyEntityWithAdditionalOwners, MyModel>(additionalUserContext.Object);
            
        RepositoryAction<MyEntityWithAdditionalOwners, MyModel> repositoryAction 
            = new(null, createEntity ? new MyEntityWithAdditionalOwners
            {
                EntityId = "main-owner",
                UserId = "main-user-id",
                AirslipUserType = AirslipUserType.Administrator,
                AdditionalOwners = new List<AdditionalOwner>()
                {
                    new()
                    {
                        EntityId = entityId,
                        UserId = userId,
                        AirslipUserType = airslipUserType
                    }
                }
            } : null, null, lifecycleStage, null);

        List<ValidationResultMessageModel> validationResult = await validateEvent
            .Validate(repositoryAction);

        validationResult.Count.Should().Be(resultCount);
        if (expectedMessage != null)
            validationResult.Count(o => o.Message.Equals(expectedMessage)).Should().Be(1);
    }
    
    
}