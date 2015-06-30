using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs.Correspondence;
using Telerik.Windows.Controls;
using Telerik.Windows.Documents;
using Telerik.Windows.Documents.Model;
using Telerik.Windows.Documents.TextSearch;
using Telerik.Windows.Documents.Base;
using Telerik.Windows.Documents.FormatProviders.Txt;
using Telerik.Windows.Documents.FormatProviders.Html;
using System.ComponentModel;


namespace EventManagementSystem.Views.Core.Booking.EventBookingTabs.Correspondence
{
    /// <summary>
    /// Interaction logic for SendEmailView.xaml
    /// </summary>
    public partial class SendEventMailView : RadWindow
    {
        public readonly SendEventMailViewModel ViewModel;

        public SendEventMailView(EventModel model, CorrespondenceModel correspondence = null)
        {
            InitializeComponent();

            //ClipboardEx.ClipboardHandlers.Clear();

            //ClipboardHandler clipboardHandlerRtf = new ClipboardHandler();
            //clipboardHandlerRtf.ClipboardDataFormat = DataFormats.Rtf;
            //clipboardHandlerRtf.DocumentFormatProvider = new RtfFormatProvider();

            //ClipboardHandler clipboardHandlerHtml = new ClipboardHandler();
            //clipboardHandlerHtml.ClipboardDataFormat = DataFormats.Html;
            //clipboardHandlerHtml.DocumentFormatProvider = new HtmlFormatProvider();

            //ClipboardHandler clipboardHandlerText = new ClipboardHandler();
            //clipboardHandlerText.ClipboardDataFormat = DataFormats.Text;
            //clipboardHandlerText.DocumentFormatProvider = new TxtFormatProvider();

            //ClipboardEx.ClipboardHandlers.Add(clipboardHandlerRtf);
            //ClipboardEx.ClipboardHandlers.Add(clipboardHandlerHtml);
            //ClipboardEx.ClipboardHandlers.Add(clipboardHandlerText);

            DataContext = ViewModel = new SendEventMailViewModel(model, correspondence);

            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;

            Owner = Application.Current.MainWindow;

            Loaded += OnSendEmailViewLoaded;
        }
        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "CloseDialog")
            {
                DialogResult = true;
                Close();
            }
        }
        private void OnSendEmailViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            ViewModel.LoadData();
        }

        private void CancelOnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void DocumentsListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.Correspondence.Documents = new ObservableCollection<Document>();

            foreach (Document item in DocumentsListBox.SelectedItems)
            {
                ViewModel.Correspondence.Documents.Add(item);
            }
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            editor.Document.Selection.Clear();

            var search = new DocumentTextSearch(editor.Document);
            var lastPosition = new DocumentPosition(editor.Document);

            bool endOfDocument = false;
            bool theFirstFinding = true;

            while (!endOfDocument)
            {
                TextRange range;

                range = search.Find("#break#", theFirstFinding ? new DocumentPosition(editor.Document) : lastPosition);

                if (range != null)
                {
                    theFirstFinding = false;
                    lastPosition = range.EndPosition;

                    editor.Document.Selection.AddSelectionStart(range.StartPosition);
                    editor.Document.Selection.AddSelectionEnd(range.EndPosition);

                    var documentEditor = new RadDocumentEditor(editor.Document);
                    documentEditor.InsertLineBreak();
                }
                else
                {
                    endOfDocument = true;
                }
            }
        }
    }
}
