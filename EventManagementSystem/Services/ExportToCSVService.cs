using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Controls.Pivot.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx;
using Telerik.Windows.Documents.Spreadsheet.Model;
using RadVerticalAlignment = Telerik.Windows.Documents.Layout.RadVerticalAlignment;


namespace EventManagementSystem.Services
{
    class ExportToCSVService
    {
        private static GridViewDataControl _grid;

        public static void ExportFromGrid(GridViewDataControl grid)
        {
            _grid = grid;
            Export();
        }

        private static void Export()
        {
            var saveFileDialog = new SaveFileDialog { Filter = "CSV files (*.csv)|*.csv" };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    using (Stream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                    {
                        _grid.Export(stream, new GridViewCsvExportOptions
                            {
                                ColumnDelimiter = CultureInfo.CurrentCulture.TextInfo.ListSeparator,
                                ShowColumnHeaders = true,
                                ShowColumnFooters = false,
                                ShowGroupFooters = false,
                                Culture = CultureInfo.CurrentCulture,
                                Encoding = Encoding.UTF8,
                                Items = (IEnumerable)_grid.ItemsSource
                            });
                    }
                }

                catch (IOException)
                {
                    MessageBox.Show(String.Format(
                            "Export to file {0} was failed. File used by another process. End process and try again.",
                            saveFileDialog.FileName), "Error");
                }
            }
        }

        public static void ExportPivotToExcel(RadPivotGrid pivot)
        {        
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = "xlsx";
          //  dialog.Filter = "CSV files (*.csv)|*.csv |All Files (*.*) | *.*";

            var result = dialog.ShowDialog();
            if ((bool)result)
            {
                try
                {
                    using (var stream = dialog.OpenFile())
                    {
                        var workbook = GenerateWorkbook(pivot);

                      //  IWorkbookFormatProvider formatProvider = new CsvFormatProvider();
                        //formatProvider.Export(workbook, stream);
                        XlsxFormatProvider provider = new XlsxFormatProvider();
                        provider.Export(workbook, stream);
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
          
        }


        private static Workbook GenerateWorkbook(RadPivotGrid pivot)
        {
            var export = pivot.GenerateExport();

            Workbook workbook = new Workbook();
            workbook.History.IsEnabled = false;

            var worksheet = workbook.Worksheets.Add();

            workbook.SuspendLayoutUpdate();
            int rowCount = export.RowCount;
            int columnCount = export.ColumnCount;

            var allCells = worksheet.Cells[0, 0, rowCount - 1, columnCount - 1];
            allCells.SetFontFamily(new ThemableFontFamily(pivot.FontFamily));
            allCells.SetFontSize(12);
            allCells.SetFill(GenerateFill(pivot.Background));

            foreach (var cellInfo in export.Cells)
            {
                int rowStartIndex = cellInfo.Row;
                int rowEndIndex = rowStartIndex + cellInfo.RowSpan - 1;
                int columnStartIndex = cellInfo.Column;
                int columnEndIndex = columnStartIndex + cellInfo.ColumnSpan - 1;

                CellSelection cellSelection = worksheet.Cells[rowStartIndex, columnStartIndex];

                var value = cellInfo.Value;
                if (value != null)
                {
                    cellSelection.SetValue(Convert.ToString(value));
                    cellSelection.SetVerticalAlignment((Telerik.Windows.Documents.Spreadsheet.Model.RadVerticalAlignment) RadVerticalAlignment.Center);
                    cellSelection.SetHorizontalAlignment(GetHorizontalAlignment(cellInfo.TextAlignment));
                    int indent = cellInfo.Indent;
                    if (indent > 0)
                    {
                        cellSelection.SetIndent(indent);
                    }
                }

                cellSelection = worksheet.Cells[rowStartIndex, columnStartIndex, rowEndIndex, columnEndIndex];

                SetCellProperties(cellInfo, cellSelection);
            }

            for (int i = 0; i < columnCount; i++)
            {
                var columnSelection = worksheet.Columns[i];
                columnSelection.AutoFitWidth();

                //NOTE: workaround for incorrect autofit.
                var newWidth = worksheet.Columns[i].GetWidth().Value.Value + 15;
                columnSelection.SetWidth(new ColumnWidth(newWidth, false));
            }

            workbook.ResumeLayoutUpdate();
            return workbook;
        }

        private static RadHorizontalAlignment GetHorizontalAlignment(TextAlignment textAlignment)
        {
            switch (textAlignment)
            {
                case TextAlignment.Center:
                    return RadHorizontalAlignment.Center;

                case TextAlignment.Left:
                    return RadHorizontalAlignment.Left;

                case TextAlignment.Right:
                    return RadHorizontalAlignment.Right;

                case TextAlignment.Justify:
                default:
                    return RadHorizontalAlignment.Justify;
            }
        }

        private static void SetCellProperties(PivotExportCellInfo cellInfo, CellSelection cellSelection)
        {
            var fill = GenerateFill(cellInfo.Background);
            if (fill != null)
            {
                cellSelection.SetFill(fill);
            }

            SolidColorBrush solidBrush = cellInfo.Foreground as SolidColorBrush;
            if (solidBrush != null)
            {
                cellSelection.SetForeColor(new ThemableColor(solidBrush.Color));
            }

            if (cellInfo.FontWeight.HasValue && cellInfo.FontWeight.Value != FontWeights.Normal)
            {
                cellSelection.SetIsBold(true);
            }

            SolidColorBrush solidBorderBrush = cellInfo.BorderBrush as SolidColorBrush;
            if (solidBorderBrush != null && cellInfo.BorderThickness.HasValue)
            {
                var borderThickness = cellInfo.BorderThickness.Value;
                var color = new ThemableColor(solidBorderBrush.Color);
                //var leftBorder = new CellBorder(GetBorderStyle(borderThickness.Left), color);
                //var topBorder = new CellBorder(GetBorderStyle(borderThickness.Top), color);
                var rightBorder = new CellBorder(GetBorderStyle(borderThickness.Right), color);
                var bottomBorder = new CellBorder(GetBorderStyle(borderThickness.Bottom), color);
                var insideBorder = cellInfo.Background != null ? new CellBorder(CellBorderStyle.None, color) : null;
                cellSelection.SetBorders(new CellBorders(null, null, rightBorder, bottomBorder, insideBorder, insideBorder, null, null));
            }
        }

        private static CellBorderStyle GetBorderStyle(double thickness)
        {
            if (thickness < 1)
            {
                return CellBorderStyle.None;
            }
            else if (thickness < 2)
            {
                return CellBorderStyle.Thin;
            }
            else if (thickness < 3)
            {
                return CellBorderStyle.Medium;
            }
            else
            {
                return CellBorderStyle.Thick;
            }
        }

        private static IFill GenerateFill(Brush brush)
        {
            if (brush != null)
            {
                SolidColorBrush solidBrush = brush as SolidColorBrush;
                if (solidBrush != null)
                {
                    return PatternFill.CreateSolidFill(solidBrush.Color);
                }
            }

            return null;
        }

    }
}
