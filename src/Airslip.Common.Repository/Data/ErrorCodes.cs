namespace Airslip.Common.Repository.Data;

public static class ErrorCodes
{
    public const string ValidationFailed = "REP00001";
    public const string NotFound = "REP00002";
    public const string VerificationFailed = "REP00003";
    public const string ModelOutdated = "REP00009";
}

public static class ErrorMessages
{
    public const string LifecycleEventDoesntApply = "Invalid execution of lifecycle event";
    public const string IdFailedVerification = "Id failed verification check";
    public const string NotFound = "Entity not found";
    public const string OwnershipCannotBeVerified = "Ownership cannot be verified";
    public const string EntityDeleted = "Entity Deleted";
    public const string ModelOutdated = "Entity is newer than provided model.";
}