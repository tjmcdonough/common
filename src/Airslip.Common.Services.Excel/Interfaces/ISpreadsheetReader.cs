using System.Collections.Generic;
using System.IO;

namespace Airslip.Common.Services.Excel.Interfaces;

public interface ISpreadsheetReader
{
    List<TType> ReadDataFromSheet<TType>(Stream stream, int sheetNumber = 1, int headerRowNumber = 1);
    List<TType> ReadDataFromSheet<TType>(string filePath, int sheetNumber = 1, int headerRowNumber = 1);
}