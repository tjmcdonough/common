using Airslip.Common.Types;
using Microsoft.AspNetCore.Http;
using Moq;
using System.IO;

namespace Airslip.Common.Testing
{
    public static class FormFileMock
    {
        public static Mock<IFormFile> GenerateMockFile(string content, string fileName)
        {
            Mock<IFormFile> logoFileMock = new();
            MemoryStream ms = new();
            StreamWriter writer = new(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            logoFileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            logoFileMock.Setup(_ => _.FileName).Returns(fileName);
            logoFileMock.Setup(_ => _.Length).Returns(ms.Length);
            logoFileMock.Setup(_ => _.ContentType).Returns(MimeTypeMap.GetMimeType(Path.GetExtension(fileName)));
            return logoFileMock;
        }
    }
}