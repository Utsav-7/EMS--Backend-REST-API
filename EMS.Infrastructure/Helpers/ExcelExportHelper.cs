using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using OfficeOpenXml;

namespace EMS_Backend_Project.EMS.Infrastructure.Services
{
    public static class ExcelExportHelper
    {
        public static byte[] ExportToExcel<T>(List<T> data)
        {
            if (data == null || !data.Any()) return null;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("TimeSheet");

                // Convert list to DataTable
                var dataTable = ConvertToDataTable(data);

                // Load DataTable into worksheet
                worksheet.Cells["A1"].LoadFromDataTable(dataTable, true);

                // Apply formatting for date and time columns
                ApplyColumnFormatting(worksheet, dataTable);

                // Auto-fit columns after setting formats
                worksheet.Cells.AutoFitColumns();

                return package.GetAsByteArray();
            }
        }

        private static DataTable ConvertToDataTable<T>(List<T> data)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            // Get properties from type T
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Create columns dynamically based on property types
            Dictionary<string, Type> columnTypes = new Dictionary<string, Type>();

            foreach (var prop in properties)
            {
                Type propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                columnTypes.Add(prop.Name, propType);
                dataTable.Columns.Add(prop.Name, propType);
            }

            // Populate DataTable rows
            foreach (var item in data)
            {
                var values = properties.Select(p => p.GetValue(item, null) ?? DBNull.Value).ToArray();
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        private static void ApplyColumnFormatting(ExcelWorksheet worksheet, DataTable dataTable)
        {
            for (int col = 0; col < dataTable.Columns.Count; col++)
            {
                Type columnType = dataTable.Columns[col].DataType;

                // Apply formatting based on column data type
                if (columnType == typeof(DateTime))
                {
                    worksheet.Column(col + 1).Style.Numberformat.Format = "yyyy-mm-dd"; // Date format
                }
                else if (columnType == typeof(TimeSpan) || columnType == typeof(DateTime))
                {
                    worksheet.Column(col + 1).Style.Numberformat.Format = "hh:mm"; // Time format
                }
            }
        }
    }
}
