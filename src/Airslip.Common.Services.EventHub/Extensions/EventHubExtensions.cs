using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Airslip.Common.Services.EventHub.Extensions
{
    public static class EventHubExtensions
    {
        public static IServiceCollection AddEventHubs(this IServiceCollection services,  
            IConfiguration configuration)
        {
            return Services
                .ConfigureServices(services, configuration);
        }
        
        public static TAtt? GetAttributeByType<TAtt, TType>() where TAtt : Attribute
        {
            // Using reflection.  
            Attribute[] attrs = Attribute.GetCustomAttributes(typeof(TType));  // Reflection.  
  
            // Displaying output.  
            foreach (Attribute attr in attrs)  
            {  
                if (attr is TAtt att)
                {
                    return att;
                }  
            }

            return null;
        }
    }
}