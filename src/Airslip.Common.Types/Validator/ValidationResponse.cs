using System.Collections.Generic;

namespace Airslip.Common.Types.Validator
{
    public class ValidationResponse
    {
        private readonly List<ValidationMessageModel> _results = new();
        public bool IsValid { get; private set; } = true;
        public List<ValidationMessageModel> Messages => _results;
        public void AddMessage(string fieldName, string errorCode, string message)
        {
            Messages
                .Add(new ValidationMessageModel(fieldName, errorCode, message));

            // Assume error at this stage
            IsValid = false;
        }
    }
}