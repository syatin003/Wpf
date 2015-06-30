using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.CRM.NewEnquiryTabs.Activity;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.CRM.NewEnquiryTabs.Activity
{
    /// <summary>
    /// Interaction logic for AddActivityView.xaml
    /// </summary>
    public partial class AddActivityView : RadWindow
    {
        public readonly AddActivityViewModel ViewModel;

        public AddActivityView(EnquiryModel enquiryModel, ActivityModel activity = null)
        {
            InitializeComponent();
            DataContext = ViewModel = new AddActivityViewModel(enquiryModel, activity);

            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;

            Owner = Application.Current.MainWindow;

            Loaded += OnAddActivityViewLoaded;
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

        public AddActivityView(List<EnquiryModel> enquiries, ActivityModel activity)
        {
            InitializeComponent();
            DataContext = ViewModel = new AddActivityViewModel(enquiries, activity);

            Owner = Application.Current.MainWindow;

            Loaded += OnAddActivityViewLoaded;
        }

        private void OnAddActivityViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            ViewModel.LoadData();
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
