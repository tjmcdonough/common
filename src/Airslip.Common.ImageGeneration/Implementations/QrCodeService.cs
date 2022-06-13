using Airslip.Common.ImageGeneration.Interfaces;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Airslip.Common.ImageGeneration.Implementations
{
    public class QrCodeService : IQrCodeService
    {
        public MemoryStream GenerateQrCodeImageForAnyString(string input, int pixelsPerModule = 20)
        {
            // Generate the image
            QRCodeGenerator qrGenerator = new();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(input, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(pixelsPerModule);
            
            // Save to a memory stream
            MemoryStream memoryStream = new();
            qrCodeImage.Save(memoryStream, ImageFormat.Jpeg);
            memoryStream.Position = 0;

            return memoryStream;
        }
        
        public MemoryStream GenerateQrCodeImageForUrl(string url, int pixelsPerModule = 20)
        {
            // Generate the URL payload
            PayloadGenerator.Url generator = new(url);
            string payload = generator.ToString();

            return GenerateQrCodeImageForAnyString(payload, pixelsPerModule);
        }
    }
}