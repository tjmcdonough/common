using Airslip.Common.Types.Interfaces;

namespace Airslip.Common.Types
{
    public class Success : ISuccess
    {
        public static readonly Success Instance = new();

        private Success()
        {
        }
    }
}
