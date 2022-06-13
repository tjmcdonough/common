using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Interfaces;

namespace Airslip.Common.Repository.UnitTests.Common;

public class MyEntityWithDataSource : MyEntity, IFromDataSource
{
    public DataSources DataSource { get; set; }
    public long TimeStamp { get; set; }
}