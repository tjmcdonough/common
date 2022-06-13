using Airslip.Common.Types.Validator;
using System.Collections.Generic;

namespace Airslip.Common.Types
{
    public class ParsingResult<TType>
    {
        public ParsingResult() : this(true) { }
        public ParsingResult(bool isValid)
        {
            IsValid = isValid;
        }
        
        private readonly List<ValidationResponse> _results = new();
        public TType? ParsedObject { get; set; }
        public bool IsValid { get; private set; }
        public List<ValidationResponse> Results => _results;
        public void AddResult(ValidationResponse response)
        {
            Results.Add(response);
            IsValid = response.IsValid && IsValid;
        }

        public static ParsingResult<TType> CreateWithError(string errorCode, string errorMessage)
        {
            var transactionParsingResult = new ParsingResult<TType>(false);
            var validationResponse = new ValidationResponse();
            validationResponse.AddMessage("", errorCode, errorMessage);
            transactionParsingResult.AddResult(validationResponse);
            return transactionParsingResult;
        }
        
        public static ParsingResult<TType> CreateWithSuccess(TType parsedObject)
        {
            var transactionParsingResult = new ParsingResult<TType>
            {
                ParsedObject = parsedObject
            };
            return transactionParsingResult;
        }
    }
}