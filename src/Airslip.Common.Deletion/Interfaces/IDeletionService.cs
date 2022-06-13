using Airslip.Common.Deletion.Models;
using Airslip.Common.Types.Interfaces;
using System.Threading.Tasks;

namespace Airslip.Common.Deletion.Interfaces;

public interface IDeletionService
{
    Task<IResponse> DeleteRecord(string integration, string id, DeleteRequest requestDetails);
}