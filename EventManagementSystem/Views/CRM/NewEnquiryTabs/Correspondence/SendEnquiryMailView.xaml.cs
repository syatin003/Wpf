using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.CRM.NewEnquiryTabs.Correspondence;
using Telerik.Windows.Controls;
using System.ComponentModel;

namespace EventManagementSystem.Views.CRM.NewEnquiryTabs.Correspondence
{
    /// <summary>
    /// Interaction logic for SendEnquiryMailView.xaml
    /// </summary>
    public partial class SendEnquiryMailView : RadWindow
    {
        public readonly SendEnquiryMailViewModel ViewModel;

        public SendEnquiryMailView(EnquiryModel model, CorrespondenceModel correspondence = null)
        {
            InitializeComponent();
            DataContext = ViewModel = new SendEnquiryMailViewModel(model, correspondence);

            Owner = Application.Current.MainWindow;

            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;

            Loaded += OnSendEnquiryViewLoaded;
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "CloseDialog")
            {
                DialogResult = true;
                Close();
            }
        }

        private void OnSendEnquiryViewLoaded(object sender, RoutedEventArgs routedEventArgs)
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
    }
}
