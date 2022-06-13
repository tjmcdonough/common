using Airslip.Common.Types.Enums;
using JetBrains.Annotations;

namespace Airslip.Common.Types.Interfaces;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public interface IFromDataSource
{
    DataSources DataSource { get; set; }
    long TimeStamp { get; set; }
}