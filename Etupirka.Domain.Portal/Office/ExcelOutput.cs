using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Etupirka.Domain.Portal.Utils;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Etupirka.Domain.Portal.Office
{
    public static class ExcelOutput
    {
        /// <summary>
        /// 导出Excel到指定路径
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entitys">实体集合</param>
        /// <param name="propertyDic">字段表头映射</param>
        public static MemoryStream RenderToStream<T>(IList<T> entitys, Dictionary<string, string> propertyDic)
        {
            DataTable dt = entitys.ToDataTable<T>(propertyDic);
            return renderToExcelStream(dt);
        }

        /// <summary>
        /// 导出Excel到浏览器
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entitys">实体集合</param>
        /// <param name="propertyDic">字段表头映射</param>
        public static MemoryStream RenderToStream<T>(IList<T> entitys, Dictionary<string, DataTableExtension.PropertyConventer> propertyDic)
        {
            DataTable dt = entitys.ToDataTable(propertyDic);
            return renderToExcelStream(dt);
        }

        private static MemoryStream renderToExcelStream(DataTable table)
        {
            MemoryStream ms = new MemoryStream();

            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();

            IRow headerRow = sheet.CreateRow(0);

            //处理表头
            foreach (DataColumn column in table.Columns)
                headerRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);

            //处理数据
            int rowIndex = 1;
            foreach (DataRow row in table.Rows)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);
                foreach (DataColumn column in table.Columns)
                {
                    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                }
                rowIndex++;
            }
            autoSizeColumns(sheet);

            workbook.Write(ms);
            ms.Flush();

            return ms;
        }

        private static void autoSizeColumns(ISheet sheet)
        {
            if (sheet != null && sheet.PhysicalNumberOfRows > 0)
            {
                IRow headerRow = sheet.GetRow(0);
                for (int i = 0, l = headerRow.LastCellNum; i < l; i++)
                    sheet.AutoSizeColumn(i);
            }
        }
    }
}
