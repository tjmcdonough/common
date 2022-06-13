using Airslip.Common.Types.Enums;
using JetBrains.Annotations;

namespace Airslip.Common.Repository.Types.Interfaces;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public interface IEntityWithTimeStamp : IEntityWithId
{
    long TimeStamp { get; set; }
}