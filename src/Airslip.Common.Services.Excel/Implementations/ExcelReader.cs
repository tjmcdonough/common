using Airslip.Common.Services.Excel.Interfaces;
using Airslip.Common.Utilities;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Airslip.Common.Services.Excel.Implementations;

public class ExcelReader : ISpreadsheetReader
{
    public List<TType> ReadDataFromSheet<TType>(Stream stream, int sheetNumber = 1, int headerRowNumber = 1)
    {
        if (sheetNumber < 1) throw new ArgumentException("Sheet number must be 1 or more", nameof(sheetNumber));
        if (headerRowNumber < 1) throw new ArgumentException("Header row must be 1 or more", nameof(headerRowNumber));
        
        // Rows and sheets are 0 based
        int skipRows = headerRowNumber - 1;
        int actualSheet = sheetNumber - 1;
        
        DataTable dtTable = new();
        List<string> rowList = new();
        ISheet sheet;
        stream.Position = 0;
        XSSFWorkbook xssWorkbook = new(stream);
        sheet = xssWorkbook.GetSheetAt(actualSheet);
        IRow headerRow = sheet.GetRow(skipRows);
        int cellCount = headerRow.LastCellNum;
        for (int j = 0; j < cellCount; j++)
        {
            ICell cell = headerRow.GetCell(j);
            if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
            {
                dtTable.Columns.Add(cell.ToString());
            } 
        }
        for (int i = 1 + skipRows; i <= sheet.LastRowNum; i++)
        {
            IRow row = sheet.GetRow(i);
            if (row == null) continue;
            if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
            for (int j = row.FirstCellNum; j < cellCount; j++)
            {
                if (row.GetCell(j) != null)
                {
                    if (!string.IsNullOrEmpty(row.GetCell(j).ToString()) && !string.IsNullOrWhiteSpace(row.GetCell(j).ToString()))
                    {
                        rowList.Add(row.GetCell(j).ToString());
                    }
                }
            }
            if(rowList.Count>0)
                dtTable.Rows.Add(rowList.ToArray());
            rowList.Clear(); 
        }

        string s = JsonConvert.SerializeObject(dtTable, Formatting.Indented);

        List<TType> myList = Json.Deserialize<List<TType>>(s);
        
        return myList;
    }
    
    
    public List<TType> ReadDataFromSheet<TType>(string filePath, int sheetNumber = 1, int headerRowNumber = 1)
    {
        if (sheetNumber < 1) throw new ArgumentException("Sheet number must be 1 or more", nameof(sheetNumber));
        if (headerRowNumber < 1) throw new ArgumentException("Header row must be 1 or more", nameof(headerRowNumber));

        using FileStream stream = new FileStream(filePath, FileMode.Open);
        
        List<TType> result = ReadDataFromSheet<TType>(stream, sheetNumber, headerRowNumber);
        
        return result;
    }
}