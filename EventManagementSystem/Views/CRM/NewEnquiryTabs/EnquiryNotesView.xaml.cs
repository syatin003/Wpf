using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.ViewModels.CRM.NewEnquiryTabs;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.CRM.NewEnquiryTabs
{
    /// <summary>
    /// Interaction logic for EnquiryNotesTab.xaml
    /// </summary>
    public partial class EnquiryNotesView : UserControl
    {
        private readonly EnquiryNotesViewModel _viewModel;

        public EnquiryNotesView()
        {
            InitializeComponent();
            DataContext = _viewModel = new EnquiryNotesViewModel();

            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            Loaded += OnEnquiryNotesViewLoaded;
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            var window = RadWindow.GetParentRadWindow(this);

            if (args.PropertyName == "EnableParentWindow")
            {
                window.IsEnabled = true;
            }
            else if (args.PropertyName == "DisableParentWindow")
            {
                window.IsEnabled = false;
            }
        }

        private void OnEnquiryNotesViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var enquiryView = (NewEnquiryView)this.ParentOfType<RadWindow>();
            _viewModel.Enquiry = enquiryView.ViewModel.Enquiry;
        }
    }
}
