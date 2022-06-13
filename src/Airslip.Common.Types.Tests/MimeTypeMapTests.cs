using FluentAssertions;
using Xunit;

namespace Airslip.Common.Types.Tests
{
    public class MimeTypeMapTests
    {
        [Fact]
        public void Can_get_svg_extension_from_content_type()
        {
            string extension = MimeTypeMap.GetExtension("image/svg+xml");
            extension.Should().Be(".svg");
        }
        
        [Fact]
        public void Can_get_svg_content_type_from_extension()
        {
            string mimeType = MimeTypeMap.GetMimeType(".svg");
            mimeType.Should().Be("image/svg+xml");
        }
    }
}