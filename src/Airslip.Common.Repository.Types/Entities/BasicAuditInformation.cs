using Airslip.Common.Repository.Types.Interfaces;
using System;

namespace Airslip.Common.Repository.Types.Entities;

public class BasicAuditInformation : IEntityWithId
{
    public string Id { get; set; } = string.Empty;
        
    public string? CreatedByUserId { get; set; }
        
    public DateTime DateCreated { get; set; }
        
    public string? UpdatedByUserId { get; set; }
        
    public DateTime? DateUpdated { get; set; }
        
    public string? DeletedByUserId { get; set; }
        
    public DateTime? DateDeleted { get; set; }
}