using System;
using System.Runtime.Serialization;

namespace Airslip.Common.Repository.Exception;

[Serializable]
public class RepositoryLifecycleException : System.Exception
{
    public string ErrorCode { get; }
    public bool CanContinue { get; }

    public RepositoryLifecycleException(string errorCode, bool canContinue)
    {
        ErrorCode = errorCode;
        CanContinue = canContinue;
    }

    protected RepositoryLifecycleException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        ErrorCode = (string)info.GetValue(nameof(ErrorCode), typeof(string))!;
        CanContinue = (bool)info.GetValue(nameof(CanContinue), typeof(bool))!;
    }
}