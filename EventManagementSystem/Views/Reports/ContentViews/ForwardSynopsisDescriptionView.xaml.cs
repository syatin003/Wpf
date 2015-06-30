using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.Services;
using EventManagementSystem.ViewModels.Reports.ContentViewModels;
using Microsoft.Win32;
using Telerik.Windows.Documents.Model;
using Telerik.Windows.Documents.FormatProviders.Pdf;
using System.IO;
using Telerik.Windows.Controls.GridView;
using System;
using Telerik.Windows.Controls;
using Telerik.Windows.Documents.FormatProviders.Html;
using System.Windows.Media;
using Telerik.Windows.Documents.Layout;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EventManagementSystem.Views.Reports.ContentViews
{
    /// <summary>
    /// Interaction logic for ForwardSynopsisDescriptionView.xaml
    /// </summary>
    public partial class ForwardSynopsisDescriptionView : UserControl
    {
        private readonly ForwardSynopsisDescriptionViewModel _viewModel;

        public ForwardSynopsisDescriptionView()
        {
            InitializeComponent();
            DataContext = _viewModel = new ForwardSynopsisDescriptionViewModel();
            Loaded += OnViewLoaded;
        }

        private void OnViewLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel.LoadData();
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            RadDocument document;
            string[] values;
            GetRadDocumentAndValues(out document, out values, _viewModel.EventsGroups, _viewModel.StartDate, _viewModel.EndDate);
            PrintService.Export(document, values);
        }

        public void GetRadDocumentAndValues(out RadDocument document, out string[] values, ObservableCollection<Models.EventsGroup> eventGroups, DateTime startDate, DateTime endDate)
        {
            document = new RadDocument
            {
                SectionDefaultPageMargin = new Padding(20, 10, 20, 10),
                LayoutMode = DocumentLayoutMode.Paged,
                SectionDefaultPageOrientation = PageOrientation.Portrait
            };
            document.Style.SpanProperties.FontSize = 12;
            document.Style.ParagraphProperties.SpacingBefore = 5;
            var section = new Section();
            document.Sections.Add(section);

            foreach (var eventGroup in eventGroups)
            {
                var paragraphBorder = new Paragraph { SpacingBefore = 10, SpacingAfter = 10 };
                var separatorBorder = new System.Windows.Controls.Border { BorderThickness = new Thickness(3) };
                var colorBrush = new SolidColorBrush { Color = Colors.SkyBlue };
                separatorBorder.BorderBrush = colorBrush;
                separatorBorder.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                var container = new InlineUIContainer { Height = 3, Width = 745, UiElement = separatorBorder };
                paragraphBorder.Inlines.Add(container);
                section.Blocks.Add(paragraphBorder);



                //Add Event Date to the Heading
                var paragraphEventDate = new Paragraph();
                var span = new Span(eventGroup.EventDate.ToString("dddd dd/MM/yy"))
                {
                    FontSize = 14,
                    FontWeight = FontWeights.Bold
                };
                paragraphEventDate.Inlines.Add(span);
                section.Blocks.Add(paragraphEventDate);


                foreach (var Event in eventGroup.Events)
                {
                    var table = new Table();
                    section.Blocks.Add(table);
                    var row = new TableRow();
                    table.Rows.Add(row);
                    var cell = new TableCell();
                    row.Cells.Add(cell);
                    cell.Background = Colors.GhostWhite;
                    cell.Borders = new TableCellBorders(BorderStyle.Single, Colors.Black);
                    var paragraphEventName = new Paragraph();
                    var spanEventName = new Span((Event.EventType == null ? String.Empty : Event.EventType.Name) + ": " + Event.Name + " (" + (Event.PrimaryContact == null ? String.Empty : Event.PrimaryContact.ContactName) + ") Num of People : " + Event.Places)
                    {
                        FontWeight = FontWeights.Bold
                    };
                    paragraphEventName.Inlines.Add(spanEventName);
                    cell.Blocks.Add(paragraphEventName);

                    CreateEventGolfsBlock(Event.EventGolfs, cell);

                    CreateEventCateringsBlock(Event.EventCaterings, cell);

                    CreateEventRoomsBlock(Event.EventRooms, cell);

                    CreateEventNotesBlock(Event.EventNotes, cell);

                    section.Blocks.Add(new Paragraph());
                }
            }
            values = new string[2];
            values[0] = "Event Synopsis Report ";
            values[1] = "Synopsis for events between  " + startDate.ToString("d") + " and " + endDate.ToString("d");
        }

        private static void CreateEventGolfsBlock(List<Models.EventGolfModel> eventGolfs, TableCell cell)
        {
            if (eventGolfs.Count > 0)
            {
                var paragraphGolfOptions = new Paragraph();
                var spanGolfOptions = new Span("Golf Options")
                {
                    FontWeight = FontWeights.Bold,
                    FontStyle = FontStyles.Italic
                };
                paragraphGolfOptions.Inlines.Add(spanGolfOptions);
                paragraphGolfOptions.SpacingBefore = 10;
                cell.Blocks.Add(paragraphGolfOptions);

                foreach (var eventGolf in eventGolfs)
                {
                    if (eventGolf.EventBookedProducts.Count > 0)
                    {
                        var tableGolfEvents = new Table();
                        cell.Blocks.Add(tableGolfEvents);
                        var isFirstProduct = true;
                        foreach (var bookedProduct in eventGolf.EventBookedProducts)
                        {
                            var tableGolfRow = new TableRow();
                            tableGolfEvents.Rows.Add(tableGolfRow);
                            var tableGolfCellCommenceTime = new TableCell();
                            tableGolfRow.Cells.Add(tableGolfCellCommenceTime);
                            tableGolfCellCommenceTime.PreferredWidth = new TableWidthUnit(180);
                            if (isFirstProduct)
                            {
                                var paragraphCommence = new Paragraph();
                                var spanCommence = new Span("To commence at " + eventGolf.Time.ToString("HH:mm"));
                                paragraphCommence.Inlines.Add(spanCommence);
                                tableGolfCellCommenceTime.Blocks.Add(paragraphCommence);
                                isFirstProduct = false;
                            }
                            var tableGolfCellProductDetail = new TableCell();
                            tableGolfRow.Cells.Add(tableGolfCellProductDetail);
                            var paragraphProductDetail = new Paragraph();
                            if (bookedProduct.Product != null)
                            {
                                var spanProductDetail = new Span(bookedProduct.Quantity + " * " + bookedProduct.Product.Name + " " + eventGolf.Golf.Name);
                                paragraphProductDetail.Inlines.Add(spanProductDetail);
                            }
                            tableGolfCellProductDetail.Blocks.Add(paragraphProductDetail);
                        }
                        AddNotesWithCaption(cell, eventGolf.EventGolf.Notes, true);
                    }
                }
            }
        }

        private static void CreateEventCateringsBlock(List<Models.EventCateringModel> eventCaterings, TableCell cell)
        {
            if (eventCaterings.Count > 0)
            {
                var paragraphCateringOptions = new Paragraph();
                var spanCateringOptions = new Span("Catering Options");
                paragraphCateringOptions.SpacingBefore = 10;
                spanCateringOptions.FontWeight = FontWeights.Bold;
                spanCateringOptions.FontStyle = FontStyles.Italic;
                paragraphCateringOptions.Inlines.Add(spanCateringOptions);
                cell.Blocks.Add(paragraphCateringOptions);

                foreach (var eventCatering in eventCaterings)
                {
                    if (eventCatering.EventBookedProducts.Count > 0)
                    {
                        var tableCateringEvents = new Table();
                        cell.Blocks.Add(tableCateringEvents);
                        var isFirstProduct = true;
                        foreach (var bookedProduct in eventCatering.EventBookedProducts)
                        {
                            var tableCateringRow = new TableRow();
                            tableCateringEvents.Rows.Add(tableCateringRow);
                            var tableCateringCellServerdTime = new TableCell();
                            tableCateringRow.Cells.Add(tableCateringCellServerdTime);
                            tableCateringCellServerdTime.PreferredWidth = new TableWidthUnit(180);
                            if (isFirstProduct)
                            {
                                var paragraphServe = new Paragraph();
                                var spanServe = new Span("To be served at " + eventCatering.Time.ToString("HH:mm"));
                                paragraphServe.Inlines.Add(spanServe);
                                tableCateringCellServerdTime.Blocks.Add(paragraphServe);
                                isFirstProduct = false;
                            }
                            var tableCateringCellProductDetail = new TableCell();
                            tableCateringRow.Cells.Add(tableCateringCellProductDetail);
                            var paragraphProductDetail = new Paragraph();
                            if (bookedProduct.Product != null)
                            {
                                var spanProductDetail = new Span(bookedProduct.Quantity + " * " + bookedProduct.Product.Name);
                                paragraphProductDetail.Inlines.Add(spanProductDetail);
                            }

                            tableCateringCellProductDetail.Blocks.Add(paragraphProductDetail);
                        }
                        AddNotesWithCaption(cell, eventCatering.EventCatering.Notes, true);

                    }
                }
            }
        }

        private static void CreateEventRoomsBlock(List<Models.EventRoomModel> eventRooms, TableCell cell)
        {
            if (eventRooms.Count > 0)
            {
                var paragraphOptions = new Paragraph();
                var spanOptions = new Span("Room Options");
                paragraphOptions.SpacingBefore = 10;
                spanOptions.FontWeight = FontWeights.Bold;
                spanOptions.FontStyle = FontStyles.Italic;
                paragraphOptions.Inlines.Add(spanOptions);
                cell.Blocks.Add(paragraphOptions);

                foreach (var eventItem in eventRooms)
                {
                    var paragraphRoom = new Paragraph();
                    var spanServe = new Span(eventItem.EventRoom.Room.Name + " form " + eventItem.EventRoom.StartTime.ToString("HH:mm") + " to " + eventItem.EventRoom.EndTime.ToString("HH:mm"));
                    paragraphRoom.SpacingBefore = 10;
                    paragraphRoom.Inlines.Add(spanServe);
                    cell.Blocks.Add(paragraphRoom);

                    //Add Room Notes
                    AddNotesWithCaption(cell, eventItem.EventRoom.Notes, true);
                }
            }
        }

        private static void CreateEventNotesBlock(ObservableCollection<Models.EventNoteModel> eventNotes, TableCell cell)
        {
            if (eventNotes.Count > 0)
            {
                var paragraphOptions = new Paragraph();
                var spanOptions = new Span("Event Notes");
                paragraphOptions.SpacingBefore = 10;
                paragraphOptions.SpacingAfter = 10;
                spanOptions.FontWeight = FontWeights.Bold;
                spanOptions.FontStyle = FontStyles.Italic;
                paragraphOptions.Inlines.Add(spanOptions);
                cell.Blocks.Add(paragraphOptions);

                foreach (var eventItem in eventNotes)
                {
                    //Add Room Notes
                    AddNotesWithCaption(cell, eventItem.Note, false);
                }
            }
        }

        private static void AddNotesWithCaption(TableCell cell, String notes, Boolean withCaption)
        {
            if (!String.IsNullOrEmpty(notes))
            {
                var paraGraphNotes = new Paragraph();

                if (withCaption)
                {
                    var spanNotesCaption = new Span("Notes: ") { FontWeight = FontWeights.Bold };
                    paraGraphNotes.Inlines.Add(spanNotesCaption);

                }
                var spanNotes = new Span(notes);
                paraGraphNotes.Inlines.Add(spanNotes);
                cell.Blocks.Add(paraGraphNotes);
            }
        }

    }
}
