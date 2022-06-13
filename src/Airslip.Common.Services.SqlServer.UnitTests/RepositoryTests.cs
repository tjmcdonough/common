using Airslip.Common.Repository.Types.Constants;
using Airslip.Common.Services.SqlServer.Implementations;
using Airslip.Common.Services.SqlServer.Interfaces;
using Xunit;
using FluentAssertions;
using System;

namespace Airslip.Common.Services.SqlServer.UnitTests;

public class QueryBuilderTests
{
    [Fact]
    public void QueryBuilder_date_format_to_long_acts_as_expected()
    {
        IQueryBuilder queryBuilder = new QueryBuilder();

        long myValue = queryBuilder.GetTypedValue("2022-02-03", typeof(long), Operators.OPERATOR_AFTER);

        myValue.Should().Be(1643846400000);
    }
    
    [Fact]
    public void QueryBuilder_date_format_to_date_acts_as_expected()
    {
        IQueryBuilder queryBuilder = new QueryBuilder();

        DateTime myValue = queryBuilder.GetTypedValue("2022-02-03", typeof(DateTime), Operators.OPERATOR_AFTER);

        myValue.Should().Be(new DateTime(2022, 2, 3));
    }
    
    [Fact]
    public void QueryBuilder_fails_with_incorrect_date()
    {
        IQueryBuilder queryBuilder = new QueryBuilder();

        Action act = () => queryBuilder.GetTypedValue("22-02-03", typeof(DateTime), Operators.OPERATOR_AFTER);

        act.Should().Throw<FormatException>();
    }
}