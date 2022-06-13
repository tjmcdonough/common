using Airslip.Common.Matching.Enum;
using Airslip.Common.Types.Interfaces;

namespace Airslip.Common.Matching.Response
{
    public class QuestionResponse : ISuccess
    {
        public QuestionResponseStatus Status { get; init; }
        public string? ErrorMessage { get; init; }
        public string? QuestionId { get; set; }
        public QuestionDetailResponse? Question { get; set; }
    }
}