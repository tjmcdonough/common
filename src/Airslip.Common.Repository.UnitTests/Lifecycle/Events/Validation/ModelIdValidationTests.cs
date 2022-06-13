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

public class ModelIdValidationTests
{
    [Theory]
    [InlineData(LifecycleStage.Update, null, false, null, null, 1, ErrorMessages.IdFailedVerification)]
    [InlineData(LifecycleStage.Update, "my-id", false, null, null, 1, ErrorMessages.IdFailedVerification)]
    [InlineData(LifecycleStage.Update, "my-id", true, null, null, 1, ErrorMessages.IdFailedVerification)]
    [InlineData(LifecycleStage.Update, "my-id", true, "not-my-id", null, 1, ErrorMessages.IdFailedVerification)]
    [InlineData(LifecycleStage.Update, "my-id", true, "my-id", "some-name", 0, null)]
    [InlineData(LifecycleStage.Create, "my-id", true, "not-my-id", null, 2, ErrorMessages.LifecycleEventDoesntApply)]
    [InlineData(LifecycleStage.Get, "my-id", true, "not-my-id", null, 2, ErrorMessages.LifecycleEventDoesntApply)]
    [InlineData(LifecycleStage.Delete, "my-id", true, "not-my-id", null, 2, ErrorMessages.LifecycleEventDoesntApply)]
    public async Task Validation_acts_as_expected(
        LifecycleStage lifecycleStage, 
        string? id,
        bool createModel,
        string? modelId, 
        string name, 
        int resultCount,
        string? expectedMessage)
    {
        IModelPreValidateEvent<MyEntity, MyModel> validateEvent =
            new ModelIdValidation<MyEntity, MyModel>();
            
        RepositoryAction<MyEntity, MyModel> repositoryAction 
            = new(id, null, null, lifecycleStage, null);

        if (createModel)
        {
            repositoryAction.SetModel(new MyModel
            {
                Id = modelId,
                Name = name
            });
        }

        List<ValidationResultMessageModel> validationResult = await validateEvent
            .Validate(repositoryAction);

        validationResult.Count.Should().Be(resultCount);
        if (expectedMessage != null)
            validationResult.Count(o => o.Message.Equals(expectedMessage)).Should().Be(1);
    }
}