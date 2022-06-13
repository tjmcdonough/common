using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Extensions;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Repository.Types.Models;
using Airslip.Common.Repository.UnitTests.Common;
using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Airslip.Common.Repository.UnitTests;

public class Helpers
{
    public class StoreInMemory<TModel> : IModelDeliveryService<TModel>
        where TModel : class, IModel
    {
        public List<TModel> Models { get; } = new();

        public Task Deliver(TModel model)
        {
            Models.Add(model);
            return Task.CompletedTask;
        }
    }


    public static IServiceProvider BuildRepoProvider(string? withEntityId = null, string? withUserId = null, 
        long? withTimeStamp = null)
    {
        IServiceCollection services = new ServiceCollection();
        IConfiguration configuration = new ConfigurationBuilder().Build();
        
        services
            .AddSingleton(_ =>
            {
                Mock<IContext> mock = new();
                mock
                    .Setup(o => o.GetEntity<MyEntity>("my-id"))
                    .ReturnsAsync(new MyEntity());
                mock
                    .Setup(o => o.GetEntity<MyEntityWithTimeStamp>("my-id"))
                    .ReturnsAsync(new MyEntityWithTimeStamp());
                return mock.Object;
            })
            .AddSingleton(_ =>
            {
                Mock<IModelValidator<MyModel>> mock = new();
                mock
                    .Setup(o => o.ValidateUpdate(It.IsAny<MyModel>()))
                    .ReturnsAsync(new ValidationResultModel());
                return mock.Object;
            })
            .AddSingleton(_ =>
            {
                Mock<IModelMapper<MyModel>> mock = new();
                mock
                    .Setup(o => o.Create<MyEntity>(It.IsAny<MyModel>()))
                    .Returns(new MyEntity());
                mock
                    .Setup(o => o.Create(It.IsAny<MyEntity>()))
                    .Returns(new MyModel());
                mock
                    .Setup(o => o.Create(It.IsAny<MyEntityWithTimeStamp>()))
                    .Returns<MyEntityWithTimeStamp>((source) => new MyModel()
                    {
                        Id = source.Id,
                        Name = source.Name,
                        EntityStatus = source.EntityStatus,
                        TimeStamp = source.TimeStamp
                    });
                mock
                    .Setup(o => o.Update(It.IsAny<MyModel>(), It.IsAny<MyEntity>()))
                    .Returns(new MyEntity());
                return mock.Object;
            })
            .AddRepositories(configuration, RepositoryUserType.Manual)
            .AddScoped(_ =>
            {
                Mock<IUserContext> mockTokenDecodeService = new();
                mockTokenDecodeService.Setup(service => service.EntityId).Returns(withEntityId);
                mockTokenDecodeService.Setup(service => service.UserId).Returns(withUserId);
                mockTokenDecodeService.Setup(service => service.AirslipUserType).Returns(AirslipUserType.InternalApi);
                return mockTokenDecodeService.Object;
            })
            .AddScoped(typeof(IModelDeliveryService<>), typeof(StoreInMemory<>));

        if (withTimeStamp != null)
        {
            ServiceDescriptor? serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IDateTimeProvider));
            if (serviceDescriptor != null) 
                services.Remove(serviceDescriptor);

            Mock<IDateTimeProvider> dateTimeProvider = new();
            dateTimeProvider.Setup(o => o.GetCurrentUnixTime()).Returns(withTimeStamp.Value);
            dateTimeProvider.Setup(o => o.GetUtcNow()).Returns(DateTime.UtcNow);
            services.AddSingleton(dateTimeProvider.Object);
        }
        
        return services.BuildServiceProvider();
    }
}