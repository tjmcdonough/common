using JetBrains.Annotations;

namespace Airslip.Common.Repository.Types.Interfaces;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public interface IModelWithTimeStamp : IModelWithId
{
    long TimeStamp { get; set; }
}