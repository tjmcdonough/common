using System.IO;
using System.Threading.Tasks;

namespace Airslip.Common.Utilities.Interfaces
{
    public interface IStorage<in T> where T : class
    {
        Task SaveFileAsync(T value);
        Task<(Stream, string?)> DownloadToStreamAsync(string name);
        Task DeleteFileAsync(string name);
    }
}
