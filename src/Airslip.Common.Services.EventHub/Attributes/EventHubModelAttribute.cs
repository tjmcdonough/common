using Airslip.Common.Types.Enums;
using System;

namespace Airslip.Common.Services.EventHub.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]  
    public class EventHubModelAttribute : Attribute
    {
        public EventHubModelAttribute(string eventHubName, DataSources dataSource)
        {
            EventHubName = eventHubName;
            DataSource = dataSource;
        }
        
        public string EventHubName { get; }
        public DataSources DataSource { get; }
    }
}