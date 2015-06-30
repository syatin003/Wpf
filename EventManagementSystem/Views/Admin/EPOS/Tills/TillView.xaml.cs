using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Admin.EPOS.Tills;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Admin.EPOS.Tills
{
    /// <summary>
    /// Interaction logic for TillsView.xaml
    /// </summary>
    public partial class TillView : UserControl
    {
        private readonly TillViewModel _viewModel;

        public TillView(TillModel tillModel, ObservableCollection<TillDivisionModel> tillDivisions)
        {
            InitializeComponent();
            DataContext = _viewModel = new TillViewModel(tillModel, tillDivisions);

            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;
            Unloaded += TillView_Unloaded;

        }

        private void TillView_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel.RefreshTill();
        }
        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "RefreshTills")
            {
                var eposView = (EPOSView)this.ParentOfType<UserControl>();
                eposView.ViewModel.RefreshTills(_viewModel.Till);
            }
        }
    }
}
