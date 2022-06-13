using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Extensions;
using Airslip.Common.Repository.Implementations;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Repository.Types.Models;
using Airslip.Common.Repository.UnitTests.Common;
using Airslip.Common.Types.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Airslip.Common.Repository.UnitTests;

public class RepositoryTests
{
    [Fact]
    public async Task Timestamp_acts_as_expected_on_deleted_entity()
    {
        const long expectedTimeStamp = 123456789;
        
        IServiceProvider provider = Helpers.BuildRepoProvider(withTimeStamp: expectedTimeStamp);

        IRepository<MyEntityWithTimeStamp, MyModel> repo = provider.GetService<IRepository<MyEntityWithTimeStamp, MyModel>>()
                                              ?? throw new NotImplementedException();

        RepositoryActionResultModel<MyModel> delete = await repo.Delete("my-id");

        delete.Should().BeOfType<SuccessfulActionResultModel<MyModel>>();
        delete.PreviousVersion.Should().NotBeNull();

        IModelDeliveryService<MyModel>? deliveryService = provider.GetService<IModelDeliveryService<MyModel>>();
        deliveryService.Should().NotBeNull();

        if (deliveryService is not Helpers.StoreInMemory<MyModel> storage)
            throw new System.Exception("Something went wrong");

        storage.Models.Count.Should().Be(1);
        storage.Models.First().TimeStamp.Should().Be(expectedTimeStamp);
    }
    
    [Fact]
    public async Task Error_is_not_thrown_when_unknown_resource_is_deleted()
    {
        IServiceProvider provider = Helpers.BuildRepoProvider();

        IRepository<MyEntity, MyModel> repo = provider.GetService<IRepository<MyEntity, MyModel>>()
                                              ?? throw new NotImplementedException();

        RepositoryActionResultModel<MyModel> delete = await repo.Delete("unknown-id");

        delete.Should().BeOfType<FailedActionResultModel<MyModel>>();
        delete.ResultType.Should().Be(ResultType.NotFound);
    }
    
    [Fact]
    public async Task Error_is_not_thrown_when_ids_do_not_match()
    {
        IServiceProvider provider = Helpers.BuildRepoProvider();

        IRepository<MyEntity, MyModel> repo = provider.GetService<IRepository<MyEntity, MyModel>>()
                                              ?? throw new NotImplementedException();

        RepositoryActionResultModel<MyModel> update = await repo.Update("unknown-id", new MyModel()
        {
            Id = "not-my-id"
        });

        update.Should().BeOfType<FailedActionResultModel<MyModel>>();
        update.ResultType.Should().Be(ResultType.FailedVerification);
    }
    
    [Fact]
    public async Task Error_is_not_thrown_when_id_not_found_for_update()
    {
        IServiceProvider provider = Helpers.BuildRepoProvider();

        IRepository<MyEntity, MyModel> repo = provider.GetService<IRepository<MyEntity, MyModel>>()
                                              ?? throw new NotImplementedException();

        RepositoryActionResultModel<MyModel> update = await repo.Update("unknown-id", new MyModel()
        {
            Id = "unknown-id"
        });

        update.Should().BeOfType<FailedActionResultModel<MyModel>>();
        update.ResultType.Should().Be(ResultType.NotFound);
    }
        
    [Fact]
    public void Can_construct_repository_with_no_delivery_service()
    {
        Mock<IContext> mockContext = new();
        Mock<IModelValidator<MyModel>> mockModelValidator = new();
        Mock<IModelMapper<MyModel>> mockModelMapper = new();
        Mock<IUserContext> mockTokenDecodeService = new();
        IConfiguration configuration = new ConfigurationBuilder().Build();
        
        IServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton(mockModelValidator.Object);
        serviceCollection.AddSingleton(mockModelMapper.Object);
        serviceCollection.AddSingleton(mockContext.Object);
        serviceCollection.AddSingleton(mockTokenDecodeService.Object);
        serviceCollection.AddSingleton(mockTokenDecodeService.Object);
        serviceCollection.AddRepositories(configuration, RepositoryUserType.Manual);
            
        var provider = serviceCollection.BuildServiceProvider();
        var myRepo = provider.GetService<IRepository<MyEntity, MyModel>>();

        myRepo.Should().NotBeNull();
    }
        
    [Fact]
    public void Can_construct_repository_with_delivery_service()
    {
        Mock<IContext> mockContext = new();
        Mock<IModelValidator<MyModel>> mockModelValidator = new();
        Mock<IModelMapper<MyModel>> mockModelMapper = new();
        Mock<IUserContext> mockTokenDecodeService = new();
        IConfiguration configuration = new ConfigurationBuilder().Build();

        IServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton(mockModelValidator.Object);
        serviceCollection.AddSingleton(mockModelMapper.Object);
        serviceCollection.AddSingleton(mockContext.Object);
        serviceCollection.AddSingleton(mockTokenDecodeService.Object);
        serviceCollection.AddSingleton(mockTokenDecodeService.Object);
        serviceCollection.AddSingleton(typeof(IModelDeliveryService<>), 
            typeof(NullModelDeliveryService<>));
        serviceCollection.AddRepositories(configuration, RepositoryUserType.Manual);
            
        var provider = serviceCollection.BuildServiceProvider();
        var myRepo = provider.GetService<IRepository<MyEntity, MyModel>>();

        myRepo.Should().NotBeNull();
    }
        
    [Fact]
    public void Can_construct_repository_logger_with_no_settings()
    {
        Mock<IContext> mockContext = new();
        Mock<IModelValidator<MyModel>> mockModelValidator = new();
        Mock<IModelMapper<MyModel>> mockModelMapper = new();
        Mock<IUserContext> mockTokenDecodeService = new();
        Mock<ILogger> logger = new();
        IConfiguration configuration = new ConfigurationBuilder().Build();

        IServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton(logger.Object);
        serviceCollection.AddSingleton(mockModelValidator.Object);
        serviceCollection.AddSingleton(mockModelMapper.Object);
        serviceCollection.AddSingleton(mockContext.Object);
        serviceCollection.AddSingleton(mockTokenDecodeService.Object);
        serviceCollection.AddSingleton(mockTokenDecodeService.Object);
        serviceCollection.AddSingleton(typeof(IModelDeliveryService<>), 
            typeof(NullModelDeliveryService<>));
        serviceCollection.AddRepositories(configuration, RepositoryUserType.Manual);
            
        ServiceProvider provider = serviceCollection.BuildServiceProvider();
        IRepositoryMetricService? myRepo = provider.GetService<IRepositoryMetricService>();

        myRepo.Should().NotBeNull();
    }
}