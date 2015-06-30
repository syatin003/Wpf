using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.ViewModels.CRM.NewEnquiryTabs;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.CRM.NewEnquiryTabs
{
    /// <summary>
    /// Interaction logic for EnquiryCorrespondenceView.xaml
    /// </summary>
    public partial class EnquiryCorrespondenceView : UserControl
    {
        private readonly EnquiryCorrespondenceViewModel _viewModel;

        public EnquiryCorrespondenceView()
        {
            InitializeComponent();
            DataContext = _viewModel = new EnquiryCorrespondenceViewModel();

            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            Loaded += OnEventCorrespondenceViewLoaded;
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

        private void OnEventCorrespondenceViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var enquiryView = (NewEnquiryView)this.ParentOfType<RadWindow>();
            _viewModel.Enquiry = enquiryView.ViewModel.Enquiry;
        }
    }
}
