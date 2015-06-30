using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.ViewModels.CRM.NewEnquiryTabs;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.CRM.NewEnquiryTabs
{
    /// <summary>
    /// Interaction logic for EnquiryActivityView.xaml
    /// </summary>
    public partial class EnquiryActivityView : UserControl
    {
        private readonly EnquiryActivityViewModel _viewModel;

        public EnquiryActivityView()
        {
            InitializeComponent();
            DataContext = _viewModel = new EnquiryActivityViewModel();

            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            Loaded += OnEnquiryDetailsViewLoaded;
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

        private void OnEnquiryDetailsViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var enquiryView = (NewEnquiryView)this.ParentOfType<RadWindow>();
            _viewModel.Enquiry = enquiryView.ViewModel.Enquiry;
           // _viewModel.LoadData();
        }
    }
}
