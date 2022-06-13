using System;
using System.Collections.Generic;

namespace Airslip.Common.Types
{
    public static class MimeTypeMap
    {
        private static readonly Lazy<IDictionary<string, string>> Mappings = new(BuildMappings);

        private static IDictionary<string, string> BuildMappings()
        {
            Dictionary<string, string> source = new(StringComparer.InvariantCultureIgnoreCase)
            {
                { "image/svg+xml", ".svg" },
                { "application/pdf", ".pdf" },
                { "image/png", ".png" },
                { "image/jpeg", ".jpg" },
                { ".svg", "image/svg+xml" },
                { ".pdf", "application/pdf" },
                { ".png", "image/png" },
                { ".jpg", "image/jpeg" }
            };

            return source;
        }

        public static string GetMimeType(string extension)
        {
            if (extension == null)
                throw new ArgumentNullException(nameof(extension));
            if (!extension.StartsWith("."))
                extension = "." + extension;
            return !Mappings.Value.TryGetValue(extension, out string? str) ? "application/octet-stream" : str;
        }

        public static string GetExtension(string mimeType)
        {
            if (mimeType == null)
                throw new ArgumentNullException(nameof(mimeType));
            if (mimeType.StartsWith("."))
                throw new ArgumentException($"Requested mime type is not valid: {mimeType}");
            if (Mappings.Value.TryGetValue(mimeType, out string? str))
                return str;
            throw new ArgumentException($"Requested mime type is not registered: {mimeType}");
        }
    }
}