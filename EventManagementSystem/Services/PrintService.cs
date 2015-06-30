using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Documents.FormatProviders;
using Telerik.Windows.Documents.FormatProviders.Html;
using Telerik.Windows.Documents.FormatProviders.Pdf;
using Telerik.Windows.Documents.Layout;
using Telerik.Windows.Documents.Model;

namespace EventManagementSystem.Services
{
    public static class PrintService
    {
        public static void Export(GridViewDataControl grid, params string[] titles)
        {
            var filePath = Path.GetTempFileName().Replace(".tmp", ".pdf");
            var document = CreateDocument(grid);

            var documentHeader = CreateHeader(titles);
            document.Sections.First.Headers.Default = new Header() { Body = documentHeader };

            if (document != null)
            {
                document.LayoutMode = DocumentLayoutMode.Paged;
                document.Measure(RadDocument.MAX_DOCUMENT_SIZE);
                document.SectionDefaultPageMargin = new Padding(40, 60, 30, 30);

                document.Arrange(new RectangleF(PointF.Empty, document.DesiredSize));

                IDocumentFormatProvider provider = new PdfFormatProvider();

                using (Stream stream = new FileStream(filePath, FileMode.Create))
                {
                    provider.Export(document, stream);
                }
            }

            Process.Start(filePath);
        }



        public static void Export(RadDocument document, params string[] titles)
        {
            var filePath = Path.GetTempFileName().Replace(".tmp", ".pdf");

            var documentHeader = CreateHeader(titles);
            document.Sections.First.Headers.Default = new Header() { Body = documentHeader };

            if (document != null)
            {
                document.LayoutMode = DocumentLayoutMode.Paged;
                document.Measure(RadDocument.MAX_DOCUMENT_SIZE);
                document.SectionDefaultPageMargin = new Padding(40, 60, 30, 30);

                document.Arrange(new RectangleF(PointF.Empty, document.DesiredSize));

                IDocumentFormatProvider provider = new PdfFormatProvider();

                using (Stream stream = new FileStream(filePath, FileMode.Create))
                {
                    provider.Export(document, stream);
                }
            }

            Process.Start(filePath);
        }

        private static RadDocument CreateHeader(params string[] titles)
        {
            RadDocument document = new RadDocument();

            Paragraph titleParagraph = new Paragraph();
            titleParagraph.TextAlignment = RadTextAlignment.Center;
            Span titleSpan = new Span(titles.First());
            titleParagraph.Inlines.Add(titleSpan);

            document.Sections.Add(new Section());
            document.Sections.First.Blocks.Add(titleParagraph);

            if (titles.Count() > 1)
            {
                for (int i = 1; i < titles.Count(); i++)
                {
                    var paragraph = new Paragraph();
                    paragraph.TextAlignment = RadTextAlignment.Center;
                    Span span = new Span(titles[i]);
                    paragraph.Inlines.Add(span);
                    document.Sections.First.Blocks.Add(paragraph);
                }
            }

            Paragraph emptyParagraph = new Paragraph();
            document.Sections.First.Blocks.Add(emptyParagraph);

            return document;
        }

        private static RadDocument CreateDocument(GridViewDataControl grid)
        {
            RadDocument document;

            using (var stream = new MemoryStream())
            {
                EventHandler<GridViewElementExportingEventArgs> elementExporting = (s, e) =>
                {
                    if (e.Element == ExportElement.Table)
                    {
                        e.Attributes["border"] = "0";
                    }
                };

                grid.ElementExporting += elementExporting;

                grid.Export(stream, new GridViewExportOptions
                {
                    Format = ExportFormat.Html,
                    ShowColumnFooters = grid.ShowColumnFooters,
                    ShowColumnHeaders = grid.ShowColumnHeaders,
                    ShowGroupFooters = grid.ShowGroupFooters
                });

                grid.ElementExporting -= elementExporting;

                stream.Position = 0;

                document = new HtmlFormatProvider().Import(stream);

                foreach (Span span in document.EnumerateChildrenOfType<Span>())
                {
                    span.FontSize = 12;
                    span.FontFamily = new FontFamily("Arial");
                }
            }

            return document;
        }

        public static void ExportPivotToPdf(RadPivotGrid pivot, params string[] titles)
        {
            var filePath = Path.GetTempFileName().Replace(".tmp", ".pdf");
            RadDocument document = GenerateRadDocument(pivot);
            var documentHeader = CreateHeader(titles);
            document.Sections.First.Headers.Default = new Header() { Body = documentHeader };
            var provider = new PdfFormatProvider();
            using (Stream stream = new FileStream(filePath, FileMode.Create))
            {
                provider.Export(document, stream);
            }
            Process.Start(filePath);
        }

        private static RadDocument GenerateRadDocument(RadPivotGrid pivot)
        {
            var export = pivot.GenerateExport();
            int rowCount = export.RowCount;
            int columnCount = export.ColumnCount;

            RadDocument document = new RadDocument();
            document.SectionDefaultPageMargin = new Padding(10);
            document.LayoutMode = DocumentLayoutMode.Paged;
            document.SectionDefaultPageOrientation = PageOrientation.Landscape;
            document.Style.SpanProperties.FontFamily = pivot.FontFamily;
            document.Style.SpanProperties.FontSize = pivot.FontSize;
            document.Style.ParagraphProperties.SpacingAfter = 0;

            var section = new Section();
            document.Sections.Add(section);
            section.Blocks.Add(new Paragraph());

            var table = new Table(rowCount, columnCount);
            section.Blocks.Add(table);

            var tableRows = table.Rows.ToArray();
            foreach (var cellInfo in export.Cells)
            {
                int rowStartIndex = cellInfo.Row;
                int rowEndIndex = rowStartIndex + cellInfo.RowSpan - 1;
                int columnStartIndex = cellInfo.Column;
                int columnEndIndex = columnStartIndex + cellInfo.ColumnSpan - 1;

                var value = cellInfo.Value;
                var text = Convert.ToString(value);
                if (!string.IsNullOrWhiteSpace(text))
                {
                    var cells = tableRows[rowStartIndex].Cells.ToArray();
                    var cell = cells[columnStartIndex];
                    Paragraph paragraph = new Paragraph();
                    cell.Blocks.Add(paragraph);
                    var span = new Span(text);
                    paragraph.Inlines.Add(span);
                    paragraph.TextAlignment = GetTextAlignment(cellInfo.TextAlignment);

                    if (cellInfo.FontWeight.HasValue)
                    {
                        span.FontWeight = cellInfo.FontWeight.Value;
                    }

                    Color foreColor;
                    if (GetColor(cellInfo.Foreground, out foreColor))
                    {
                        span.ForeColor = foreColor;
                    }

                    cell.VerticalAlignment = GetVerticalAlignment(cellInfo.VerticalAlignment);
                    paragraph.LeftIndent = cellInfo.Indent * 20;
                }

                var borderThickness = cellInfo.BorderThickness;
                var borderBrush = cellInfo.BorderBrush;
                var background = cellInfo.Background;

                Color backColor;
                bool hasBackground = GetColor(cellInfo.Background, out backColor);

                if (cellInfo.RowSpan > 1 && cellInfo.ColumnSpan > 1)
                {
                    for (int k = rowStartIndex; k <= rowEndIndex; k++)
                    {
                        var cells = tableRows[k].Cells.ToArray();
                        for (int j = columnStartIndex; j <= columnEndIndex; j++)
                        {
                            var cell = cells[j];
                            if (hasBackground)
                            {
                                cell.Background = backColor;
                            }

                            cell.Borders = GetCellBorders(borderThickness, borderBrush, cell.Borders, k, rowStartIndex, rowEndIndex, j, columnStartIndex, columnEndIndex, hasBackground);
                        }

                    }
                }
                else if (cellInfo.RowSpan > 1)
                {
                    for (int j = rowStartIndex; j <= rowEndIndex; j++)
                    {
                        // TODO: check when ColumnSpan > 1;
                        var cell = tableRows[j].Cells.ToArray()[columnStartIndex];

                        Position position = j == rowStartIndex ? Position.First : ((j == rowEndIndex) ? Position.Last : Position.Middle);

                        cell.Borders = GetCellBorders(borderThickness, borderBrush, position, cell.Borders, true, cellInfo.Background != null);
                        if (hasBackground)
                        {
                            cell.Background = backColor;
                        }
                    }
                }
                else if (cellInfo.ColumnSpan > 1)
                {
                    var cells = tableRows[rowStartIndex].Cells.ToArray();
                    for (int j = columnStartIndex; j <= columnEndIndex; j++)
                    {
                        // TODO: check when RowSpan > 1;
                        var cell = cells[j];

                        Position position = j == columnStartIndex ? Position.First : ((j == columnEndIndex) ? Position.Last : Position.Middle);
                        if (hasBackground)
                        {
                            cell.Background = backColor;
                        }

                        cell.Borders = GetCellBorders(borderThickness, borderBrush, position, cell.Borders, false, hasBackground);
                    }
                }
            }

            return document;
        }

        private static bool GetColor(Brush brush, out Color color)
        {
            SolidColorBrush solidBrush = brush as SolidColorBrush;
            if (solidBrush != null)
            {
                color = solidBrush.Color;
                return true;
            }

            color = Colors.White;
            return false;
        }

        private static TableCellBorders GetCellBorders(Thickness? borderThickness, Brush borderBrush, TableCellBorders cellBorders,
            int rowIndex, int rowStartIndex, int rowEndIndex, int columnIndex, int columnStartIndex, int columnEndIndex, bool hasBackground)
        {
            Color borderBrushColor;
            GetColor(borderBrush, out borderBrushColor);

            if (!borderThickness.HasValue)
            {
                return new TableCellBorders(new Telerik.Windows.Documents.Model.Border(BorderStyle.None));
            }

            var thickness = borderThickness.Value;
            Telerik.Windows.Documents.Model.Border topBorder = cellBorders.Top;
            Telerik.Windows.Documents.Model.Border bottomBorder = cellBorders.Bottom;
            Telerik.Windows.Documents.Model.Border leftBorder = cellBorders.Left;
            Telerik.Windows.Documents.Model.Border rightBorder = cellBorders.Right;

            if (rowIndex == rowStartIndex)
            {
                topBorder = new Telerik.Windows.Documents.Model.Border((float)thickness.Top, BorderStyle.Single, borderBrushColor);
            }

            if (rowIndex == rowEndIndex)
            {
                bottomBorder = new Telerik.Windows.Documents.Model.Border((float)thickness.Bottom, BorderStyle.Single, borderBrushColor);
            }

            if (rowStartIndex < rowIndex && rowIndex < rowEndIndex)
            {
                topBorder = hasBackground ? new Telerik.Windows.Documents.Model.Border(BorderStyle.None) : cellBorders.Top;
                bottomBorder = hasBackground ? new Telerik.Windows.Documents.Model.Border(BorderStyle.None) : cellBorders.Bottom;
            }

            if (columnIndex == columnStartIndex)
            {
                leftBorder = new Telerik.Windows.Documents.Model.Border((float)thickness.Left, BorderStyle.Single, borderBrushColor);
            }

            if (columnIndex == columnEndIndex)
            {
                rightBorder = new Telerik.Windows.Documents.Model.Border((float)thickness.Right, BorderStyle.Single, borderBrushColor);
            }

            if (columnStartIndex < columnIndex && columnIndex < columnEndIndex)
            {
                leftBorder = hasBackground ? new Telerik.Windows.Documents.Model.Border(BorderStyle.None) : cellBorders.Left;
                rightBorder = hasBackground ? new Telerik.Windows.Documents.Model.Border(BorderStyle.None) : cellBorders.Right;
            }

            return new TableCellBorders(leftBorder, topBorder, rightBorder, bottomBorder);
        }

        private static TableCellBorders GetCellBorders(Thickness? borderThickness, Brush borderBrush, Position position, TableCellBorders cellBorders, bool isRow, bool hasBackground)
        {
            Color borderBrushColor;
            GetColor(borderBrush, out borderBrushColor);

            if (!borderThickness.HasValue)
            {
                return new TableCellBorders(new Telerik.Windows.Documents.Model.Border(BorderStyle.None));
            }

            var thickness = borderThickness.Value;
            if (isRow)
            {
                var leftBorder = new Telerik.Windows.Documents.Model.Border((float)thickness.Left, BorderStyle.Single, borderBrushColor);
                var rightBorder = new Telerik.Windows.Documents.Model.Border((float)thickness.Right, BorderStyle.Single, borderBrushColor);
                Telerik.Windows.Documents.Model.Border topBorder;
                Telerik.Windows.Documents.Model.Border bottomBorder;
                switch (position)
                {
                    case Position.First:
                        topBorder = new Telerik.Windows.Documents.Model.Border((float)thickness.Top, BorderStyle.Single, borderBrushColor);
                        bottomBorder = hasBackground ? new Telerik.Windows.Documents.Model.Border(BorderStyle.None) : cellBorders.Bottom;
                        break;

                    case Position.Middle:
                        topBorder = hasBackground ? new Telerik.Windows.Documents.Model.Border(BorderStyle.None) : cellBorders.Top;
                        bottomBorder = hasBackground ? new Telerik.Windows.Documents.Model.Border(BorderStyle.None) : cellBorders.Bottom;
                        break;

                    case Position.Last:
                    default:
                        topBorder = hasBackground ? new Telerik.Windows.Documents.Model.Border(BorderStyle.None) : cellBorders.Top;
                        bottomBorder = new Telerik.Windows.Documents.Model.Border((float)thickness.Bottom, BorderStyle.Single, borderBrushColor);
                        break;
                }

                return new TableCellBorders(leftBorder, topBorder, rightBorder, bottomBorder);
            }
            else
            {
                var topBorder = new Telerik.Windows.Documents.Model.Border((float)thickness.Top, BorderStyle.Single, borderBrushColor);
                var bottomBorder = new Telerik.Windows.Documents.Model.Border((float)thickness.Bottom, BorderStyle.Single, borderBrushColor);
                Telerik.Windows.Documents.Model.Border leftBorder;
                Telerik.Windows.Documents.Model.Border rightBorder;
                switch (position)
                {
                    case Position.First:
                        leftBorder = new Telerik.Windows.Documents.Model.Border((float)thickness.Left, BorderStyle.Single, borderBrushColor);
                        rightBorder = hasBackground ? new Telerik.Windows.Documents.Model.Border(BorderStyle.None) : cellBorders.Right;
                        break;

                    case Position.Middle:
                        leftBorder = hasBackground ? new Telerik.Windows.Documents.Model.Border(BorderStyle.None) : cellBorders.Left;
                        rightBorder = hasBackground ? new Telerik.Windows.Documents.Model.Border(BorderStyle.None) : cellBorders.Right;
                        break;

                    case Position.Last:
                    default:
                        leftBorder = hasBackground ? new Telerik.Windows.Documents.Model.Border(BorderStyle.None) : cellBorders.Left; ;
                        rightBorder = new Telerik.Windows.Documents.Model.Border((float)thickness.Right, BorderStyle.Single, borderBrushColor);
                        break;
                }

                return new TableCellBorders(leftBorder, topBorder, rightBorder, bottomBorder);
            }
        }

        private static RadVerticalAlignment GetVerticalAlignment(VerticalAlignment verticalAlignment)
        {
            switch (verticalAlignment)
            {
                case VerticalAlignment.Bottom:
                    return Telerik.Windows.Documents.Layout.RadVerticalAlignment.Bottom;

                case VerticalAlignment.Stretch:
                case VerticalAlignment.Center:
                    return Telerik.Windows.Documents.Layout.RadVerticalAlignment.Center;

                case VerticalAlignment.Top:
                default:
                    return Telerik.Windows.Documents.Layout.RadVerticalAlignment.Top;
            }
        }

        private static RadTextAlignment GetTextAlignment(TextAlignment textAlignment)
        {
            switch (textAlignment)
            {
                case TextAlignment.Center:
                    return Telerik.Windows.Documents.Layout.RadTextAlignment.Center;

                case TextAlignment.Justify:
                    return Telerik.Windows.Documents.Layout.RadTextAlignment.Justify;

                case TextAlignment.Right:
                    return Telerik.Windows.Documents.Layout.RadTextAlignment.Right;

                case TextAlignment.Left:
                default:
                    return Telerik.Windows.Documents.Layout.RadTextAlignment.Left;
            }
        }

        private enum Position
        {
            First,
            Middle,
            Last
        }
    }


}
