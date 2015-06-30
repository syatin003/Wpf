using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Events;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace EventManagementSystem.Views.Events
{
    /// <summary>
    /// Interaction logic for RemindersView.xaml
    /// </summary>
    public partial class RemindersView
    {
        private readonly RemindersViewModel _viewModel;

        public RemindersView()
        {
            InitializeComponent();
            DataContext = _viewModel = new RemindersViewModel();

            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            RemindersRadGridView.MouseDoubleClick += RemindersRadGridView_MouseDoubleClick;

            IsVisibleChanged += OnIsVisibleChanged;
        }

        private void RemindersRadGridView_MouseDoubleClick(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            var originalSender = mouseButtonEventArgs.OriginalSource as FrameworkElement;
            if (originalSender != null)
            {
                var row = originalSender.ParentOfType<GridViewRow>();
                if (row != null)
                {
                    _viewModel.EditReminderCommand.Execute(row.Item as EventReminderModel);
                }
            }
        }
        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (IsVisible)
                _viewModel.LoadData();
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
