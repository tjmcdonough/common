using Airslip.Common.Repository.Data;
using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Implementations.Events.Entity.PreValidate;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Repository.Types.Models;
using Airslip.Common.Repository.UnitTests.Common;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Airslip.Common.Repository.UnitTests.Lifecycle.Events.Validation;

public class EntityStatusValidationTests
{
    [Theory]
    [InlineData(LifecycleStage.Update, true, EntityStatus.Deleted, 2, ErrorMessages.LifecycleEventDoesntApply)]
    [InlineData(LifecycleStage.Update, true, EntityStatus.Active, 1, ErrorMessages.LifecycleEventDoesntApply)]
    [InlineData(LifecycleStage.Update, false, EntityStatus.Deleted, 1, ErrorMessages.LifecycleEventDoesntApply)]
    [InlineData(LifecycleStage.Create, false, EntityStatus.Active, 1, ErrorMessages.LifecycleEventDoesntApply)]
    [InlineData(LifecycleStage.Get, true, EntityStatus.Deleted, 1, ErrorMessages.EntityDeleted)]
    [InlineData(LifecycleStage.Delete, true, EntityStatus.Deleted, 2, ErrorMessages.LifecycleEventDoesntApply)]
    [InlineData(LifecycleStage.Get, true, EntityStatus.Active, 0, null)]
    [InlineData(LifecycleStage.Delete, true, EntityStatus.Active, 1, ErrorMessages.LifecycleEventDoesntApply)]
    public async Task Validation_acts_as_expected(
        LifecycleStage lifecycleStage, 
        bool createEntity,
        EntityStatus entityStatus,
        int resultCount,
        string? expectedMessage)
    {
        IEntityPreValidateEvent<MyEntity, MyModel> validateEvent =
            new EntityStatusValidation<MyEntity, MyModel>();
            
        RepositoryAction<MyEntity, MyModel> repositoryAction 
            = new(null, createEntity ? new MyEntity
            {
                EntityStatus = entityStatus
            } : null, null, lifecycleStage, null);

        List<ValidationResultMessageModel> validationResult = await validateEvent
            .Validate(repositoryAction);

        validationResult.Count.Should().Be(resultCount);
        if (expectedMessage != null)
            validationResult.Count(o => o.Message.Equals(expectedMessage)).Should().Be(1);
    }
}