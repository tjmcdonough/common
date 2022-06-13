using Airslip.Common.Services.Excel.Implementations;
using Airslip.Common.Services.Excel.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Airslip.Common.Services.Excel;

public static class Services
{
    public static IServiceCollection AddExcelReader(this IServiceCollection serviceCollection)
    {
        return serviceCollection
                .AddScoped<ISpreadsheetReader, ExcelReader>();
    }
}