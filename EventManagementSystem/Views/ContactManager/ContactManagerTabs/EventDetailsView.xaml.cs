using System.ComponentModel;
using System.Windows;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.ContactManager.ContactManagerTabs;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.ContactManager.ContactManagerTabs
{
    /// <summary>
    /// Interaction logic for EventDetailsView.xaml
    /// </summary>
    public partial class EventDetailsView : RadWindow
    {
        private readonly EventDetailsViewModel _viewModel;

        public EventDetailsView(EventModel model)
        {
            InitializeComponent();
            DataContext = _viewModel = new EventDetailsViewModel(model);

            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            Owner = Application.Current.MainWindow;

            Loaded += EventDetailsViewLoaded;
        }

        private void EventDetailsViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
           // _viewModel.LoadData();
            _viewModel.LoadLightEventDetails();
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

        private void EditEvent_OnClick(object sender, RoutedEventArgs e)
        {
            var eventModel = ((RadButton)sender).Tag as EventModel;
            _viewModel.EditEventCommand.Execute(eventModel);
        }
    }
}
