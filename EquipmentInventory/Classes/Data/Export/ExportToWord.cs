using EquipmentInventory.Classes.Helper;
using EquipmentInventory.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Word = Microsoft.Office.Interop.Word;

namespace EquipmentInventory.Classes.Data.Export;

public static class ExportToWord
{
    private const int FontSize = 11;
    private const string FontFamily = "Times New Roman";

    public static bool Export<T>(List<T> data)
    {
        var saveFileDialog = new SaveFileDialog
        {
            Filter = "Word Documents (*.docx)|*.docx",
            DefaultExt = ".docx",
            FileName = $"{Strings.Report}.docx",
            Title = $"{Strings.TitleSaveReport} Word"
        };

        if (saveFileDialog.ShowDialog() != true)
            return false;

        string filePath = saveFileDialog.FileName;

        Word.Application wordApp = null;
        Word.Document document = null;

        try
        {
            wordApp = new Word.Application();
            wordApp.Visible = false;

            document = wordApp.Documents.Add();

            // Настройка страницы
            document.PageSetup.Orientation = Word.WdOrientation.wdOrientLandscape;
            document.PageSetup.TopMargin = wordApp.CentimetersToPoints(1.5f);
            document.PageSetup.BottomMargin = wordApp.CentimetersToPoints(1.5f);
            document.PageSetup.LeftMargin = wordApp.CentimetersToPoints(1.5f);
            document.PageSetup.RightMargin = wordApp.CentimetersToPoints(1.5f);

            // Добавляем заголовок документа
            Word.Paragraph title = document.Paragraphs.Add();
            title.Range.Text = Strings.Report;
            title.Range.Font.Name = FontFamily;
            title.Range.Font.Size = 16;
            title.Range.Font.Bold = 1;
            title.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            title.Range.InsertParagraphAfter();

            // Создаем таблицу
            var properties = typeof(T).GetProperties();
            Word.Table table = document.Tables.Add(
                document.Content,
                data.Count + 1, 
                properties.Length
            );

            // Стиль таблицы
            table.Borders.Enable = 1;
            table.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            table.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleDouble;
            table.Borders.InsideLineWidth = Word.WdLineWidth.wdLineWidth050pt;
            table.Borders.OutsideLineWidth = Word.WdLineWidth.wdLineWidth075pt;
            table.AllowAutoFit = true;

            // Форматирование заголовков
            for (int col = 0; col < properties.Length; col++)
            {
                table.Cell(1, col + 1).Range.Text = properties[col].Name;
                table.Cell(1, col + 1).Range.Font.Bold = 1;
                table.Cell(1, col + 1).Range.Font.Name = FontFamily;
                table.Cell(1, col + 1).Range.Font.Size = FontSize;
                table.Cell(1, col + 1).Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                table.Cell(1, col + 1).Range.Shading.BackgroundPatternColor = Word.WdColor.wdColorGray15;
            }

            // Заполнение данных
            for (int row = 0; row < data.Count; row++)
            {
                for (int col = 0; col < properties.Length; col++)
                {
                    var cell = table.Cell(row + 2, col + 1);
                    object value = properties[col].GetValue(data[row]);

                    cell.Range.Font.Bold = 0;
                    cell.Range.Font.Name = FontFamily;
                    cell.Range.Font.Size = FontSize;

                    // Специальное форматирование для разных типов данных
                    if (value is decimal || value is double || value is float)
                    {
                        cell.Range.Text = string.Format("{0:#,##0.00}", value);
                        cell.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                    }
                    else if (value is DateTime)
                    {
                        cell.Range.Text = ((DateTime)value).ToString("dd.MM.yyyy");
                        cell.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    }
                    else
                    {
                        cell.Range.Text = value?.ToString() ?? "";
                        cell.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                    }
                }
            }

            // Автоподбор ширины
            table.AutoFitBehavior(Word.WdAutoFitBehavior.wdAutoFitWindow);

            // Добавляем подпись
            Word.Paragraph footer = document.Paragraphs.Add();
            footer.Range.InsertParagraphBefore();
            footer.Range.Text = $"{Strings.DescriptionReport} {DateTime.Now:dd.MM.yyyy}";
            footer.Range.Font.Name = FontFamily;
            footer.Range.Font.Size = FontSize;
            footer.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;

            // Сохранение
            document.SaveAs2(filePath);
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
            if (document != null)
            {
                document.Close(false);
                Marshal.ReleaseComObject(document);
            }
            if (wordApp != null)
            {
                wordApp.Quit();
                Marshal.ReleaseComObject(wordApp);
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
