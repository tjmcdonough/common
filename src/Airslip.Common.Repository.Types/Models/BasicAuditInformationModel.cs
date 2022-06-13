using System;

namespace Airslip.Common.Repository.Types.Models;

public class BasicAuditInformationModel
{
    public string? CreatedByUserId { get; set; }
        
    public DateTime DateCreated { get; set; }
        
    public string? UpdatedByUserId { get; set; }
        
    public DateTime? DateUpdated { get; set; }
        
    public string? DeletedByUserId { get; set; }
        
    public DateTime? DateDeleted { get; set; }
}