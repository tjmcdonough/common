namespace Airslip.Common.Types.Validator
{
    public static class InvalidConstants
    {
        public const string ErrorCode = "INVALID_ATTRIBUTE";
        public const string Message = "Invalid {PropertyName}. '{PropertyValue}' {Validation}.";
        public const string OneOf = "must be one of: ";
        public const string DateTimeValidation = "must be a valid datetime representing UTC with timezone";
        public const string UrlValidation = "must be a valid url";
        public const string DecimalValidation = "must be a valid decimal";
        public const string MustBeGreaterThanZero = "must be a value higher than 0";
        public const string MustBeValidEmail = "must be a valid email";
        public const string PropertyMustBeOneOf = "{PropertyName} must be one of ";
        
        public const string PreviouslyAuthorisedConsent =
            "The user must have a previously authorised consent for this institution. Use CreateAccountAuthorisation endpoint and complete the process to retrieve";

        public const string UnhandledError =
            "A problem has happened our end, the issue has been logged and someone from our team is looking at it";
    }

    public static class InvalidConstantsForEach
    {
        public const string ErrorCode = "INVALID_ATTRIBUTE";

        public const string Message =
            "Invalid {PropertyName} at index {CollectionIndex}. '{PropertyValue}' {Validation}.";

        public const string OneOf = "must be one of: ";
    }

    public static class RequiredConstants
    {
        public const string ErrorCode = "REQUIRED_ATTRIBUTE";
        public const string Message = "The attribute {PropertyName} is required.";
    }

    public static class RequiredConstantsForEach
    {
        public const string ErrorCode = "REQUIRED_ATTRIBUTE";
        public const string Message = "The attribute {PropertyName} at index {CollectionIndex} is required.";
    }

    public static class ResourceNotFoundConstants
    {
        public const string ErrorCode = "RESOURCE_NOT_FOUND";
        public const string Message = "The resource could not be found. {Validation}.";
        public const string MustBeAValidIdentifier = "Must be a valid identifier";
        public const string CreateUserMustBeCalled = "CreateUser endpoint must be called first";
        public const string ErrorCreatingUser = "Error creating user";
    }

    public static class InvalidResourceConstants
    {
        public const string UseCreateAccountAuthorisation =
            "Use CreateAccountAuthorisation endpoint and complete the process to retrieve";
    }
}