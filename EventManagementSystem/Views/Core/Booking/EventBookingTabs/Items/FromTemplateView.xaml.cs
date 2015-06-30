using System.Windows;
using Telerik.Windows.Controls;
using System.ComponentModel;
using System.Collections.Generic;
using EventManagementSystem.ViewModels.Core.Booking.EventBookingTabs.Items;
using EventManagementSystem.Models;

namespace EventManagementSystem.Views.Core.Booking.EventBookingTabs.Items
{
    /// <summary>
    /// Interaction logic for MailFieldsView.xaml
    /// </summary>
    public partial class FromTemplateView : RadWindow
    {
        public readonly FromTemplateViewModel ViewModel;

        public FromTemplateView(EventModel eventModel)
        {
            InitializeComponent();
            DataContext = ViewModel = new FromTemplateViewModel(eventModel);

            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;
            Owner = Application.Current.MainWindow;

            Loaded += FromTemplateView_Loaded;
        }
        private void FromTemplateView_Loaded(object sender, RoutedEventArgs routedEventArgs)
        {
            ViewModel.LoadData();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "CloseDialog")
            {
                DialogResult = true;
                Close();
            }
            else if (args.PropertyName == "EnableParentWindow")
            {
                this.IsEnabled = true;
            }
            else if (args.PropertyName == "DisableParentWindow")
            {
                this.IsEnabled = false;
            }
        }
    }
}
