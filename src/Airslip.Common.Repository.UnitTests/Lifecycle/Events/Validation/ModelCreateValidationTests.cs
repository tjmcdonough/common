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

public class ModelCreateValidationTests
{
    [Theory]
    [InlineData(LifecycleStage.Create, "Some name", 0, null)]
    [InlineData(LifecycleStage.Create, "", 1, null)]
    [InlineData(LifecycleStage.Create, "01234567890123456789Invalid", 1, null)]
    [InlineData(LifecycleStage.Update, "Some name", 1, ErrorMessages.LifecycleEventDoesntApply)]
    [InlineData(LifecycleStage.Delete, "Some name", 1, ErrorMessages.LifecycleEventDoesntApply)]
    [InlineData(LifecycleStage.Get, "Some name", 1, ErrorMessages.LifecycleEventDoesntApply)]
    public async Task Validation_acts_as_expected(LifecycleStage lifecycleStage, string name, int resultCount,
        string? expectedMessage)
    {
        IModelPreValidateEvent<MyEntity, MyModel> validateEvent =
            new ModelCreateValidation<MyEntity, MyModel>(new MyModelValidator());

        MyModel myModel = new()
        {
            Name = name
        };

        RepositoryAction<MyEntity, MyModel> repositoryAction 
            = new(null, null, myModel, lifecycleStage, null);

        List<ValidationResultMessageModel> validationResult = await validateEvent
            .Validate(repositoryAction);

        validationResult.Count.Should().Be(resultCount);
        if (expectedMessage != null)
            validationResult.Count(o => o.Message.Equals(expectedMessage)).Should().Be(1);
    }
}