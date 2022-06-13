using Airslip.Common.Types.Interfaces;

namespace Airslip.Common.Deletion.Models;

public record DeleteResponse(string Id) : ISuccess;