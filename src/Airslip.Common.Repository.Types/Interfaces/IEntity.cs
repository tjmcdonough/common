using Airslip.Common.Repository.Types.Entities;
using Airslip.Common.Repository.Types.Enums;

namespace Airslip.Common.Repository.Types.Interfaces;

/// <summary>
/// A simple interface defining the common data properties for basic auditing of changes to an entity object
/// </summary>
public interface IEntity : IEntityWithId
{
    BasicAuditInformation? AuditInformation { get; set; }
        
    EntityStatus EntityStatus { get; set; }
}