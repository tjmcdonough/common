using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Repository.Types.Interfaces;

namespace Airslip.Common.Repository.UnitTests.Common;

public class MyModelWithTimeStamp : IModelWithTimeStamp, IModel
{
    public string? Id { get; set; }
    public EntityStatus EntityStatus { get; set; }
    public string Name { get; set; } = string.Empty;
    public long TimeStamp { get; set; }
}