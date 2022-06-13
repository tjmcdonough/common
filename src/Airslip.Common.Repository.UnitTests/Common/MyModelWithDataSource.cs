using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Interfaces;

namespace Airslip.Common.Repository.UnitTests.Common;

public class MyModelWithDataSource : MyModel, IFromDataSource
{
    public DataSources DataSource { get; set; }
    public long TimeStamp { get; set; }
}