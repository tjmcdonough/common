using Airslip.Common.Repository.Types.Constants;

namespace Airslip.Common.Repository.Types.Models;

public record SearchFilterModel(string ColumnField, dynamic? Value, 
    string OperatorValue = Operators.OPERATOR_EQUALS);