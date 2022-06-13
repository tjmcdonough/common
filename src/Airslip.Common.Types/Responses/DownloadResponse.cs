using Airslip.Common.Types.Interfaces;

namespace Airslip.Common.Types.Responses;

public record DownloadResponse(string FileName, byte[] FileContent, 
    string MediaType) : ISuccess;