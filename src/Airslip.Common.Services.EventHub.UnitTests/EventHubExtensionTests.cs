using Airslip.Common.Services.EventHub.Attributes;
using Airslip.Common.Services.EventHub.Extensions;
using Airslip.Common.Types.Enums;
using FluentAssertions;
using JetBrains.Annotations;
using Xunit;

namespace Airslip.Common.Services.EventHub.UnitTests
{
    public class EventHubExtensionTests
    {
        private const string eventHubName = "event-hub-name";
        
        [Fact]
        public void Type_with_attribute_returns_correct_value()
        {
            EventHubModelAttribute? theAttribute = EventHubExtensions
                .GetAttributeByType<EventHubModelAttribute, MyTypeWithAttribute>();
            theAttribute.Should().NotBeNull();
            theAttribute!.EventHubName.Should().Be(eventHubName);
        }
        [Fact]
        public void Type_with_no_attribute_returns_null()
        {
            EventHubModelAttribute? theAttribute = EventHubExtensions
                .GetAttributeByType<EventHubModelAttribute, MyTypeWithoutAttribute>();
            theAttribute.Should().BeNull();
        }

        [EventHubModel(eventHubName, DataSources.Yapily)]
        [UsedImplicitly]
        private class MyTypeWithAttribute
        {
            
        }
        
        [UsedImplicitly]
        private class MyTypeWithoutAttribute
        {
            
        }
    }
}