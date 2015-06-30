using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EventManagementSystem.Data.Model;
using EventManagementSystem.ViewModels.Admin.Members.Tokens;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Admin.Members.Tokens
{
    /// <summary>
    /// Interaction logic for MembershipTokensView.xaml
    /// </summary>
    public partial class MembershipTokensView : UserControl
    {
        private readonly MembershipTokensViewModel _viewModel;

        public MembershipTokensView()
        {
            InitializeComponent();
            _viewModel = new MembershipTokensViewModel();
            DataContext = _viewModel;

            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            Loaded += OnViewLoaded;
        }

        private void OnViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Loaded -= OnViewLoaded;
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
            else if (args.PropertyName == "SetFocusOnTokenText")
            {
                txtToken.CaretIndex = txtToken.Text.Length;
                txtToken.Focus();
            }
        }

        private void RadListBoxTokens_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var originalSender = e.OriginalSource as FrameworkElement;
            if (originalSender != null)
            {
                var item = originalSender.ParentOfType<RadListBoxItem>();
                if (item != null)
                    _viewModel.EditMembershipTokenCommand.Execute(item.DataContext as MembershipToken);
            }
        }
    }
}
