using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.ViewModels.Core;
using Telerik.Windows;
using Telerik.Windows.Controls;
using System.ComponentModel;

namespace EventManagementSystem.Views.Core
{
    /// <summary>
    /// Interaction logic for WorkspaceView.xaml
    /// </summary>
    public partial class WorkspaceView : UserControl
    {
        private readonly WorkspaceViewModel _viewModel;

        public WorkspaceView()
        {
            InitializeComponent();
            DataContext = _viewModel = new WorkspaceViewModel();
            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;
            Loaded += OnWorkspaceViewLoaded;
        }

        private void OnWorkspaceViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.OnLoadData();
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
        private void RadTileView_OnTileStateChanged(object sender, RadRoutedEventArgs e)
        {
            var item = e.OriginalSource as RadTileViewItem;
            if (item != null)
            {
                var fluid = item.ChildrenOfType<RadFluidContentControl>().FirstOrDefault();
                if (fluid != null)
                {
                    switch (item.TileState)
                    {
                        case TileViewItemState.Maximized:
                            fluid.State = FluidContentControlState.Large;
                            break;
                        case TileViewItemState.Minimized:
                            fluid.State = FluidContentControlState.Normal;
                            break;
                        case TileViewItemState.Restored:
                            fluid.State = FluidContentControlState.Normal;
                            break;
                    }
                }
            }
        }
    }
}
