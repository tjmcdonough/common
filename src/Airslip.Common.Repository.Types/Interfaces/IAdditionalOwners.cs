using Airslip.Common.Repository.Types.Entities;
using System.Collections.Generic;

namespace Airslip.Common.Repository.Types.Interfaces;

public interface IAdditionalOwners
{
    ICollection<AdditionalOwner> AdditionalOwners { get; set; }
}