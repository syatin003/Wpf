using EventManagementSystem.Data.Model;
using EventManagementSystem.ViewModels.Admin.Settings;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Admin.Settings
{
    /// <summary>
    /// Interaction logic for EmailHeadersView.xaml
    /// </summary>
    public partial class EmailHeadersView : UserControl
    {
        private readonly EmailHeadersViewModel _viewModel;

        public EmailHeadersView()
        {
            InitializeComponent();
            DataContext = _viewModel = new EmailHeadersViewModel();
            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            Loaded += EmailHeadersView_Loaded;
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            var tile = this.ParentOfType<RadTileView>();

            if (args.PropertyName == "EnableParentWindow")
            {
                tile.IsEnabled = true;
            }
            else if (args.PropertyName == "DisableParentWindow")
            {
                tile.IsEnabled = false;
            }
        }
        private void EmailHeadersView_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= EmailHeadersView_Loaded;
            _viewModel.LoadData();
        }
    }
}
