using Airslip.Common.Repository.Data;
using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Implementations.Events.Model.PreValidate;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Types.Models;
using Airslip.Common.Repository.UnitTests.Common;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Airslip.Common.Repository.UnitTests.Lifecycle.Events.Validation;

public class IdRequiredValidationTests
{
    [Theory]
    [InlineData(LifecycleStage.Update, null, 1, ErrorMessages.IdFailedVerification)]
    [InlineData(LifecycleStage.Update, "my-id", 0, null)]
    [InlineData(LifecycleStage.Create, "my-id", 1, ErrorMessages.LifecycleEventDoesntApply)]
    [InlineData(LifecycleStage.Get, "my-id", 0, null)]
    [InlineData(LifecycleStage.Delete, "my-id", 0, null)]
    public async Task Validation_acts_as_expected(
        LifecycleStage lifecycleStage, 
        string? id,
        int resultCount,
        string? expectedMessage)
    {
        IModelPreValidateEvent<MyEntity, MyModel> validateEvent =
            new IdRequiredValidation<MyEntity, MyModel>();
            
        RepositoryAction<MyEntity, MyModel> repositoryAction 
            = new(id, null, null, lifecycleStage, null);

        List<ValidationResultMessageModel> validationResult = await validateEvent
            .Validate(repositoryAction);

        validationResult.Count.Should().Be(resultCount);
        if (expectedMessage != null)
            validationResult.Count(o => o.Message.Equals(expectedMessage)).Should().Be(1);
    }
}