using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using EventManagementSystem.Services;
using EventManagementSystem.ViewModels.Reports.ContentViewModels;
using Telerik.Windows.Controls;
using EventManagementSystem.Models;

namespace EventManagementSystem.Views.Reports.ContentViews
{
    /// <summary>
    /// Interaction logic for CalendarDescriptionView.xaml
    /// </summary>
    public partial class CalendarDescriptionView
    {
        private readonly CalendarDescriptionViewModel _viewModel;

        public CalendarDescriptionView()
        {
            InitializeComponent();
            DataContext = _viewModel = new CalendarDescriptionViewModel();

            Loaded += OnViewLoaded;
        }

        public void OnViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.LoadData();
        }


        private void Print_OnClick(object sender, RoutedEventArgs e)
        {
            GenerateReport(CalenderRadGridView, _viewModel.StartDate, _viewModel.EndDate);
        }

        public void GenerateReport(RadGridView radGrid, DateTime startDate, DateTime endDate)
        {
            var values = new string[2];
            values[0] = "Calender Report";
            values[1] = "Between " + startDate.ToString("d") + " and " + endDate.ToString("d");
            // values[2] = _viewModel.EnabledItems.Count > 0 ? "including events of type " + string.Join(",", _viewModel.EnabledItems.Select(x => x.ToString()).ToArray()) : "";
            PrintService.Export(radGrid, values);
        }

        private void Export_OnClick(object sender, RoutedEventArgs e)
        {
            ExportToCSVService.ExportFromGrid(CalenderRadGridView);
        }
        public void LoadData(List<EventModel> allEvents, List<EventTypeModel> allEventTypes, DateTime startDate, DateTime endDate)
        {
            _viewModel.LoadOptions();
            _viewModel.RefreshEventTypes(allEventTypes);
            _viewModel.UpdateCalenderDataRange(allEvents, startDate, endDate);
            var radGridView = new RadGridView { ItemsSource = _viewModel.Events };
            PopulateRadGridView(radGridView);
            GenerateReport(radGridView, startDate, endDate);
        }

        private void PopulateRadGridView(RadGridView radGridView)
        {
            var dateColumn = new GridViewDataColumn
            {
                Header = "Date",
                DataMemberBinding = new Binding("Date"),
                DataFormatString = "ddddd dd/MM/yy",
                IsVisible = _viewModel.IncEventDate
            };
            radGridView.Columns.Add(dateColumn);
            var nameColumn = new GridViewDataColumn();
            dateColumn.DataMemberBinding = new Binding("Date");
            nameColumn.Header = "Event Name";
            nameColumn.DataMemberBinding = new Binding("Event.Name");
            radGridView.Columns.Add(nameColumn);
            var typeColumn = new GridViewDataColumn { Header = "Type", DataMemberBinding = new Binding("EventType.Name") };
            radGridView.Columns.Add(typeColumn);
            var contactColumn = new GridViewDataColumn();
            contactColumn.Header = "Primary Contact";
            contactColumn.DataMemberBinding = new Binding("PrimaryContact.ContactName");
            contactColumn.IsVisible = _viewModel.IncPrimaryContact;
            radGridView.Columns.Add(contactColumn);
            var placesColumn = new GridViewDataColumn
            {
                Header = "Places",
                DataMemberBinding = new Binding("Event.Places"),
                IsVisible = _viewModel.IncPlaces
            };
            radGridView.Columns.Add(placesColumn);
            var statusColumn = new GridViewDataColumn
            {
                Header = "Status",
                DataMemberBinding = new Binding("EventStatus.Name"),
                IsVisible = _viewModel.IncStatus
            };
            radGridView.Columns.Add(statusColumn);
            var changesColumn = new GridViewDataColumn
            {
                Header = "Changes",
                DataMemberBinding = new Binding("Changes"),
                IsVisible = _viewModel.IncChanges
            };
            radGridView.Columns.Add(changesColumn);
            var startTimeColumn = new GridViewDataColumn
            {
                Header = "Start Time",
                DataMemberBinding = new Binding("StartTime"),
                IsVisible = _viewModel.IncStartTime
            };
            radGridView.Columns.Add(startTimeColumn);
            var phoneColumn = new GridViewDataColumn
            {
                Header = "Telephone",
                DataMemberBinding = new Binding("PrimaryContact.Contact.Phone1"),
                IsVisible = _viewModel.IncTelNumbers
            };
            radGridView.Columns.Add(phoneColumn);
            var emailColumn = new GridViewDataColumn
            {
                Header = "Email",
                DataMemberBinding = new Binding("PrimaryContact.Contact.Email"),
                IsVisible = _viewModel.IncEmail
            };
            radGridView.Columns.Add(emailColumn);
            radGridView.AutoGenerateColumns = false;
        }
    }
}
