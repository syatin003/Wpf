using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.CRM.NewEnquiryTabs.FollowUp;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.CRM.NewEnquiryTabs.FollowUp
{
    /// <summary>
    /// Interaction logic for AddFollowUpView.xaml
    /// </summary>
    public partial class AddFollowUpView : RadWindow
    {
        public readonly AddFollowUpViewModel ViewModel;

        public AddFollowUpView(FollowUpModel followUp = null)
        {
            InitializeComponent();
            DataContext = ViewModel = new AddFollowUpViewModel(followUp);

            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;

            Owner = Application.Current.MainWindow;
            Header = "Add TO DO";
            Loaded += OnAddFollowUpViewLoaded;
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

        public AddFollowUpView(EnquiryModel enquiryModel, bool addToActivity, FollowUpModel followUp = null)
        {
            InitializeComponent();
            DataContext = ViewModel = new AddFollowUpViewModel(enquiryModel, addToActivity, followUp);

            Owner = Application.Current.MainWindow;

            Loaded += OnAddFollowUpViewLoaded;
        }

        public AddFollowUpView(List<EnquiryModel> enquiries, FollowUpModel followUp = null)
        {
            InitializeComponent();
            DataContext = ViewModel = new AddFollowUpViewModel(enquiries, followUp);

            Owner = Application.Current.MainWindow;

            Loaded += OnAddFollowUpViewLoaded;
        }

        private void OnAddFollowUpViewLoaded(object sender, RoutedEventArgs routedEventArgs)
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
