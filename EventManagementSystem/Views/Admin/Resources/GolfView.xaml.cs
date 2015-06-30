using System.Windows.Controls;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Admin.Resources;
using System.ComponentModel;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Admin.Resources
{
    /// <summary>
    /// Interaction logic for GolfView.xaml
    /// </summary>
    public partial class GolfView : UserControl
    {
        private readonly GolfViewModel _viewModel;

        public GolfView(GolfModel golfModel)
        {
            InitializeComponent();
            DataContext = _viewModel = new GolfViewModel(golfModel);
            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;
            Unloaded += GolfView_Unloaded;
        }

        private void GolfView_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel.Refresh();
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


    }
}
