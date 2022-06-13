namespace Airslip.Common.Repository.Types.Interfaces;

/// <summary>
/// A simple interface defining the common data properties for basic auditing of changes to an entity object
/// </summary>
public interface IEntityWithId
{
    string Id { get; set; }
}