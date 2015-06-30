using System.Windows;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.CRM.NewEnquiryTabs.Notes;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.CRM.NewEnquiryTabs.Notes
{
    /// <summary>
    /// Interaction logic for AddNoteView.xaml
    /// </summary>
    public partial class AddNoteView : RadWindow
    {
        private readonly AddEnquiryNoteViewModel _viewModel;

        public AddNoteView(EnquiryModel enquiryModel, EnquiryNoteModel note = null)
        {
            InitializeComponent();
            DataContext = _viewModel = new AddEnquiryNoteViewModel(enquiryModel, note);

            Owner = Application.Current.MainWindow;

            Loaded += OnAddNoteViewLoaded;
        }

        private void OnAddNoteViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.LoadData();
        }

        private void OnSubmitButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
