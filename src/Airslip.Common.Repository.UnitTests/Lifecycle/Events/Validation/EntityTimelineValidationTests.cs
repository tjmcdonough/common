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

public class EntityTimelineValidationTests
{
    [Theory]
    [InlineData(LifecycleStage.Update, 2, 1, 1, ErrorMessages.ModelOutdated)]
    [InlineData(LifecycleStage.Update, 1, 2, 0, null)]
    [InlineData(LifecycleStage.Update, 1, 1, 1, ErrorMessages.ModelOutdated)]
    [InlineData(LifecycleStage.Create, 1, 2, 1, ErrorMessages.LifecycleEventDoesntApply)]
    [InlineData(LifecycleStage.Get, 1, 2, 1, ErrorMessages.LifecycleEventDoesntApply)]
    [InlineData(LifecycleStage.Delete, 1, 2, 1, ErrorMessages.LifecycleEventDoesntApply)]
    public async Task Validation_acts_as_expected(
        LifecycleStage lifecycleStage, 
        long entityTimestamp,
        long modelTimestamp,
        int resultCount,
        string? expectedMessage)
    {
        IEntityPreValidateEvent<MyEntity, MyModel> validateEvent =
            new EntityTimelineValidation<MyEntity, MyModel>();
            
        RepositoryAction<MyEntity, MyModel> repositoryAction 
            = new(null, new MyEntityWithDataSource
            {
                TimeStamp = entityTimestamp
            }, new MyModelWithDataSource
            {
                TimeStamp = modelTimestamp
            }, lifecycleStage, null);

        List<ValidationResultMessageModel> validationResult = await validateEvent
            .Validate(repositoryAction);

        validationResult.Count.Should().Be(resultCount);
        if (expectedMessage != null)
            validationResult.Count(o => o.Message.Equals(expectedMessage)).Should().Be(1);
    }
    
    [Fact]
    public async Task Validation_doesnt_apply_to_non_data_source_model()
    {
        IEntityPreValidateEvent<MyEntity, MyModel> validateEvent =
            new EntityTimelineValidation<MyEntity, MyModel>();
            
        RepositoryAction<MyEntity, MyModel> repositoryAction 
            = new(null, new MyEntity(), new MyModel(), LifecycleStage.Update, null);

        List<ValidationResultMessageModel> validationResult = await validateEvent
            .Validate(repositoryAction);

        validationResult.Count.Should().Be(0);
    }
}