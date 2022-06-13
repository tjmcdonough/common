using Airslip.Common.Matching.Enum;
using System.Collections.Generic;

namespace Airslip.Common.Matching.Response
{
    public class QuestionDetailResponse
    {
        public TransactionQuestionType QuestionType { get; set; }
        
        public string? QuestionText { get; init; }
        
        public List<string>? Options { get; init; }
    }
}