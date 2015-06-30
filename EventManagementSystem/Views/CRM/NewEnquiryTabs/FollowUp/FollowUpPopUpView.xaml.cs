using System.ComponentModel;
using System.Windows;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.CRM.NewEnquiryTabs.FollowUp;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.CRM.NewEnquiryTabs.FollowUp
{
    /// <summary>
    /// Interaction logic for FollowUpPopUp.xaml
    /// </summary>
    public partial class FollowUpPopUp : RadWindow
    {
        private readonly FollowUpPopUpViewModel _viewModel;

        public FollowUpPopUp(FollowUpModel followUp)
        {
            InitializeComponent();
            DataContext = _viewModel = new FollowUpPopUpViewModel(followUp);

            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            Owner = Application.Current.MainWindow;
            Loaded += OnFollowUpPopUpViewLoaded;
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
            else if (args.PropertyName == "CloseParentWindow")
            {
                Close();
            }
        }

        private void OnFollowUpPopUpViewLoaded(object sender, RoutedEventArgs routedEventArgs)
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
        }
    }
}
