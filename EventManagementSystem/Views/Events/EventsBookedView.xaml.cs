using System.Windows;
using EventManagementSystem.ViewModels.Events;
using Telerik.Windows.Controls;
using System.ComponentModel;
using EventManagementSystem.Models;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace EventManagementSystem.Views.Events
{
    /// <summary>
    /// Interaction logic for EventsBookedView.xaml
    /// </summary>
    public partial class EventsBookedView : RadWindow
    {
        public readonly EventsBookedViewModel ViewModel;

        public EventsBookedView(System.Collections.ObjectModel.ObservableCollection<Models.EventModel> bookedEvents)
        {
            InitializeComponent();
            DataContext = ViewModel = new EventsBookedViewModel(bookedEvents);
            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;
            Owner = Application.Current.MainWindow;
            GridView.MouseDoubleClick += GridViewOnMouseDoubleClick;
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


        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void GridViewOnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var originalSender = e.OriginalSource as FrameworkElement;
            if (originalSender != null)
            {
                var row = originalSender.ParentOfType<GridViewRow>();
                if (row != null)
                {
                    ViewModel.DetailsItemCommand.Execute(row.Item as EventModel);
                }
            }
        }

     
    }
}
