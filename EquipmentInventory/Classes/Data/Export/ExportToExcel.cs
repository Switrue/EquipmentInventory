using EquipmentInventory.Classes.Helper;
using EquipmentInventory.Properties;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace EquipmentInventory.Classes.Data.Export;

public static class ExportToExcel
{
    public static bool Export<T>(List<T> data)
    {
        var saveFileDialog = new SaveFileDialog
        {
            Filter = "Excel Files (*.xlsx)|*.xlsx",
            DefaultExt = ".xlsx",
            FileName = $"{Strings.Report}.xlsx",
            Title = $"{Strings.TitleSaveReport} Excel"
        };

        if (saveFileDialog.ShowDialog() != true)
            return false;

        string filePath = saveFileDialog.FileName;
        string tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".xlsx");

        Excel.Application excelApp = null;
        Workbook workbook = null;

        try
        {
            excelApp = new Excel.Application
            {
                Visible = false,
                DisplayAlerts = false
            };

            workbook = excelApp.Workbooks.Add();
            Worksheet worksheet = (Worksheet)workbook.Sheets[1];
            worksheet.Name = Strings.Data;

            // Получаем свойства для заголовков
            var properties = typeof(T).GetProperties();

            // Форматирование заголовков
            Range headerRange = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, properties.Length]];
            headerRange.Font.Bold = true;
            headerRange.Interior.Color = XlRgbColor.rgbLightGray;
            headerRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            headerRange.Borders.LineStyle = XlLineStyle.xlContinuous;
            headerRange.Borders.Weight = XlBorderWeight.xlThin;

            // Заполнение данных
            for (int col = 0; col < properties.Length; col++)
            {
                worksheet.Cells[1, col + 1] = properties[col].Name;

                for (int row = 0; row < data.Count; row++)
                {
                    var cell = (Range)worksheet.Cells[row + 2, col + 1];
                    object value = properties[col].GetValue(data[row]);

                    if (value is decimal || value is double || value is float)
                    {
                        cell.NumberFormat = "#,##0.00";
                        cell.HorizontalAlignment = XlHAlign.xlHAlignRight;
                    }
                    else if (value is DateTime)
                    {
                        cell.NumberFormat = "dd.MM.yyyy";
                        cell.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    }

                    cell.Value = value;
                    cell.Borders.LineStyle = XlLineStyle.xlContinuous;
                }
            }

            // Автоподбор и сохранение
            worksheet.Columns.AutoFit();
            workbook.SaveAs(tempFilePath);
            workbook.Close();

            // Заменяем конечный файл
            if (File.Exists(filePath))
                File.Delete(filePath);

            File.Move(tempFilePath, filePath);

            Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
            return true;
        }
        catch (Exception ex)
        {
            CustomMessageBoxHelper.Show(Strings.Error, $"{Strings.Error}: {ex.Message}");

            return false;
        }
        finally
        {
            // Очистка временного файла
            if (File.Exists(tempFilePath))
                File.Delete(tempFilePath);

            // Освобождение ресурсов
            if (workbook != null)
                Marshal.ReleaseComObject(workbook);

            if (excelApp != null)
            {
                excelApp.Quit();
                Marshal.ReleaseComObject(excelApp);
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
