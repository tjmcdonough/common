using Airslip.Common.Repository.Data;
using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Implementations.Events.Entity.PreValidate;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Types.Models;
using Airslip.Common.Repository.UnitTests.Common;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Airslip.Common.Repository.UnitTests.Lifecycle.Events.Validation;

public class EntityFoundValidationTests
{
    [Theory]
    [InlineData(LifecycleStage.Update, false, 1, ErrorMessages.NotFound)]
    [InlineData(LifecycleStage.Update, true, 0, null)]
    [InlineData(LifecycleStage.Create, false, 2, ErrorMessages.LifecycleEventDoesntApply)]
    [InlineData(LifecycleStage.Get, true, 0, null)]
    [InlineData(LifecycleStage.Delete, true, 0, null)]
    public async Task Validation_acts_as_expected(
        LifecycleStage lifecycleStage, 
        bool createEntity,
        int resultCount,
        string? expectedMessage)
    {
        IEntityPreValidateEvent<MyEntity, MyModel> validateEvent =
            new EntityFoundValidation<MyEntity, MyModel>();
            
        RepositoryAction<MyEntity, MyModel> repositoryAction 
            = new(null, createEntity ? new MyEntity() : null, null, lifecycleStage, null);

        List<ValidationResultMessageModel> validationResult = await validateEvent
            .Validate(repositoryAction);

        validationResult.Count.Should().Be(resultCount);
        if (expectedMessage != null)
            validationResult.Count(o => o.Message.Equals(expectedMessage)).Should().Be(1);
    }
}