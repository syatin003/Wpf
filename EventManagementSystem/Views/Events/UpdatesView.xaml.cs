using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.ViewModels.Events;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Events
{
    /// <summary>
    /// Interaction logic for UpdatesView.xaml
    /// </summary>
    public partial class UpdatesView : UserControl
    {
        private readonly UpdatesViewModel _viewModel;

        public UpdatesView()
        {
            InitializeComponent();
            DataContext = _viewModel = new UpdatesViewModel();
            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            IsVisibleChanged += OnIsVisibleChanged;
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

        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (IsVisible)
                _viewModel.LoadData();
        }
    }
}
