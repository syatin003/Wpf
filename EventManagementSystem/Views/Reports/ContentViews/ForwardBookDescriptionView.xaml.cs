using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.Services;
using EventManagementSystem.ViewModels.Reports.ContentViewModels;

namespace EventManagementSystem.Views.Reports.ContentViews
{
    /// <summary>
    /// Interaction logic for ForwardBookView.xaml
    /// </summary>
    public partial class ForwardBookDescriptionView : UserControl
    {
        private readonly ForwardBookViewModel _viewModel;

        public ForwardBookDescriptionView()
        {
            InitializeComponent();
            DataContext = _viewModel = new ForwardBookViewModel();

            Loaded += OnViewLoaded;
        }

        public void OnViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.SetDescriptionActivated(false);
            _viewModel.LoadData();
            _viewModel.SetDescriptionActivated(true);

        }

        private void Print_OnClick(object sender, RoutedEventArgs e)
        {
            var values = new string[4];
            values[0] = "Forward Book Report";
            values[1] = "for events between " + _viewModel.StartDate.ToString("d") + " and " + _viewModel.EndDate.ToString("d");
            values[2] = _viewModel.GroupOption ? "Grouped by Product Group" : "Grouped by Product Department ";
            values[3] = _viewModel.IncVATOption ? "Inc V.A.T." : "Ex V.A.T.";

            PrintService.ExportPivotToPdf(ForwardBookRadPivotGrid, values);
        }

        private void Export_OnClick(object sender, RoutedEventArgs e)
        {
            ExportToCSVService.ExportPivotToExcel(ForwardBookRadPivotGrid);
        }

    }
}
