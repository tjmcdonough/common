namespace Airslip.Common.Types
{
    public static class ApiConstants
    {
        public const string VersionOne = "1.0";
        public const string CorrelationIdName = "Correlation-Id";
        public static readonly string[] ValidPatchOperations = { "add", "remove", "replace", "move", "copy", "test" };
    }
}