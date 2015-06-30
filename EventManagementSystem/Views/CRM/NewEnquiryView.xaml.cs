using System.ComponentModel;
using System.Windows;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.CRM;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.CRM
{
    /// <summary>
    /// Interaction logic for NewEnquiryView.xaml
    /// </summary>
    public partial class NewEnquiryView : RadWindow
    {
        public NewEnquiryViewModel ViewModel;

        public NewEnquiryView(EnquiryModel enquiryModel = null)
        {
            InitializeComponent();
            DataContext = ViewModel = new NewEnquiryViewModel(enquiryModel);

            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;

            Owner = Application.Current.MainWindow;
            Loaded += OnNewEnquiryViewLoaded;
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "EnableParentWindow")
            {
                this.IsEnabled = true;
            }
            else if (args.PropertyName == "DisableParentWindow")
            {
                this.IsEnabled = false;
            }
        }

        private void OnNewEnquiryViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            ViewModel.LoadData();
        }

        private void OnOkButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
