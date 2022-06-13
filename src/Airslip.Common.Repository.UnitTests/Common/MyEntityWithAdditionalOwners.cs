using Airslip.Common.Repository.Types.Entities;
using Airslip.Common.Repository.Types.Interfaces;
using System.Collections.Generic;

namespace Airslip.Common.Repository.UnitTests.Common;

public class MyEntityWithAdditionalOwners : MyEntity, IAdditionalOwners
{
    public ICollection<AdditionalOwner> AdditionalOwners { get; set; } = new List<AdditionalOwner>();
}