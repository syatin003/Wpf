using System.Windows;
using EventManagementSystem.Models;
using Telerik.Windows.Controls;
using EventManagementSystem.ViewModels.Admin.Events;
using System;


namespace EventManagementSystem.Views.Admin.Events
{
    /// <summary>
    /// Interaction logic for AddDefaultEventTypeTODOView.xaml
    /// </summary>
    public partial class AddDefaultEventTypeTODOView : RadWindow
    {
        public readonly AddDefaultEventTypeTODOViewModel ViewModel;

        public AddDefaultEventTypeTODOView(EventTypeModel eventTypeModel, EventTypeToDoModel eventTypeToDoModel = null)
        {
            InitializeComponent();
            DataContext = ViewModel = new AddDefaultEventTypeTODOViewModel(eventTypeModel, eventTypeToDoModel);
            if (eventTypeModel != null)
                this.Header = "Edit Default To Do";
            Owner = Application.Current.MainWindow;
            Loaded += OnViewLoaded;
        }

        private void OnViewLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadData();
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
