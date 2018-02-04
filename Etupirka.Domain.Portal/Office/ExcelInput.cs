using System;
using System.Data;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Etupirka.Domain.Portal.Office
{
    /// <summary>
    /// Excel导入
    /// </summary>
    public static class ExcelInput
    {
        public static DataTable GetExcel(FileInfo excelFile)
        {
            if (excelFile == null)
                throw new ArgumentNullException("excelFile");
            if (!excelFile.Exists)
                throw new FileNotFoundException("文件不存在！");

            IWorkbook workbook = null;
            string ext = excelFile.Extension.ToLower().Trim();
            if (ext == ".xlsx")
            {
                workbook = new XSSFWorkbook(new FileStream(excelFile.FullName, FileMode.Open));
            }
            else if (ext == ".xls")
            {
                workbook = new HSSFWorkbook(new FileStream(excelFile.FullName, FileMode.Open));
            }
            else
            {
                try
                {
                    workbook = new XSSFWorkbook(new FileStream(excelFile.FullName, FileMode.Open));
                }
                catch
                {
                    workbook = new HSSFWorkbook(new FileStream(excelFile.FullName, FileMode.Open));
                }
            }

            return getDataTable(workbook);
        }

        private static DataTable getDataTable(IWorkbook wb)
        {
            //ISheet sht = wb.GetSheet("Sheet1");
            ISheet sheet = wb.GetSheetAt(0);

            DataTable table = new DataTable();
            IRow headerRow = sheet.GetRow(0);//第一行为标题行  
            int cellCount = headerRow.LastCellNum;
            int rowCount = sheet.LastRowNum;

            //表头 
            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }

            //表数据
            for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = table.NewRow();
                if (row != null)
                {
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                            dataRow[j] = getCellValue(row.GetCell(j));
                    }
                }
                table.Rows.Add(dataRow);
            }
            return table;

        }

        private static string getCellValue(ICell cell)
        {
            if (cell == null)
                return string.Empty;
            switch (cell.CellType)
            {
                case CellType.Blank:
                    return string.Empty;
                case CellType.Boolean:
                    return cell.BooleanCellValue.ToString();
                case CellType.Error:
                    return cell.ErrorCellValue.ToString();
                case CellType.Numeric:
                    if (DateUtil.IsCellDateFormatted(cell))
                        return DateTime.FromOADate(cell.NumericCellValue).ToString();
                    else
                        return Convert.ToDouble(cell.NumericCellValue).ToString();
                case CellType.Unknown:
                default:
                    return cell.ToString();//This is a trick to get the correct value of the cell. NumericCellValue will return a numeric value no matter the cell value is a date or a number
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Formula:
                    try
                    {
                        HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                        e.EvaluateInCell(cell);
                        return cell.ToString();
                    }
                    catch
                    {
                        return cell.NumericCellValue.ToString();
                    }
            }
        }
    }
}
