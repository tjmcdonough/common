using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Services.AutoMapper.Extensions;
using Airslip.Common.Services.AutoMapper.Implementations;
using Airslip.Common.Utilities.Extensions;
using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Airslip.Common.Services.AutoMapper
{
    public static class Services
    {
        public static void ConfigureServices(IServiceCollection serviceCollection, 
            Action<MapperConfigurationExpression> mapperConfiguration, MapperUsageType configureFor)
        {
            serviceCollection.AddSingleton(typeof(IModelMapper<>), typeof(AutoMapperModelMapper<>));

            MapperConfigurationExpression configExpression = new();
            configExpression.IgnoreUnmapped();
            mapperConfiguration(configExpression);

            if (configureFor == MapperUsageType.Api)
            {
                configExpression.ForAllMaps((map, _) =>
                {
                    if (!typeof(IModelWithOwnership).IsAssignableFrom(map.SourceType)) return;
                
                    foreach (var prop in map.PropertyMaps
                        .Where(o => o.DestinationName
                            .InList(nameof(IModelWithOwnership.EntityId), 
                                nameof(IModelWithOwnership.AirslipUserType))))
                    {
                        prop.Ignored = true;
                    }
                });                
            }
            
            // Auto Mapper Configurations
            MapperConfiguration mappingConfig = new(configExpression);
            
            mappingConfig.AssertConfigurationIsValid();
            
            IMapper? mapper = mappingConfig.CreateMapper();
            serviceCollection.AddSingleton(mapper);
        }
    }

    public enum MapperUsageType
    {
        Api,
        Service
    }
}