using System.Windows;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.CRM;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.CRM
{
    /// <summary>
    /// Interaction logic for AddCampaignView.xaml
    /// </summary>
    public partial class AddCampaignView : RadWindow
    {
        public AddCampaignViewModel ViewModel { get; private set; }

        public AddCampaignView()
        {
            InitializeComponent();
            DataContext = ViewModel = new AddCampaignViewModel(null);

            Owner = Application.Current.MainWindow;
            Loaded += OnAddCampaignViewLoaded;
        }

        public AddCampaignView(CampaignModel model)
        {
            InitializeComponent();
            DataContext = ViewModel = new AddCampaignViewModel(model);

            Owner = Application.Current.MainWindow;
            Loaded += OnAddCampaignViewLoaded;
        }

        private void OnAddCampaignViewLoaded(object sender, RoutedEventArgs routedEventArgs)
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
