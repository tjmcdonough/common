using System.Collections.Generic;
using System.Linq;

namespace Airslip.Common.Repository.Types.Models;

public class ValidationResultModel
{
    public bool IsValid => !Results.Any();

    public List<ValidationResultMessageModel> Results { get; } = new();

    public void AddMessage(string fieldName, string message)
    {
        Results
            .Add(new ValidationResultMessageModel(fieldName, message));
    }
}