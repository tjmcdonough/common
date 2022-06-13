using Airslip.Common.Types.Enums;

namespace Airslip.Common.Types;

/// <summary>
/// A standardised address using the xNAL format <see cref="http://xml.coverpages.org/xnal.html" />
/// </summary>
public class Address
{
    /// <summary>
    /// First line of address
    /// </summary>
    public string? FirstLine { get; set; }

    /// <summary>
    /// Second line of address
    /// </summary>
    public string? SecondLine { get; set; }

    /// <summary>
    /// City / Town
    /// </summary>
    public string? Locality { get; set; }

    /// <summary>
    /// State / Province / Region
    /// </summary>
    public string? AdministrativeArea { get; set; }

    /// <summary>
    /// County / District
    /// </summary>
    public string? SubAdministrativeArea { get; set; }

    /// <summary>
    /// Postcode / ZIP Code
    /// </summary>
    public string? PostalCode { get; set; }

    /// <summary>
    /// ALPHA-2 ISO 3166 country code <see cref="https://www.iso.org/iso-3166-country-codes.html" />
    /// </summary>
    public Alpha2CountryCodes? CountryCode { get; set; }
}