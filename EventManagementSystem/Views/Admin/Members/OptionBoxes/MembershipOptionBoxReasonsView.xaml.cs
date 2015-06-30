using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EventManagementSystem.Data.Model;
using EventManagementSystem.ViewModels.Admin.Members.OptionBoxes;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Admin.Members.OptionBoxes
{
    /// <summary>
    /// Interaction logic for MembershipOptionBoxReasonsView.xaml
    /// </summary>
    public partial class MembershipOptionBoxReasonsView : UserControl
    {
        private readonly MembershipOptionBoxReasonsViewModel _viewModel;

        public MembershipOptionBoxReasonsView(MembershipOptionBox membershipOptionBox)
        {
            InitializeComponent();
            _viewModel = new MembershipOptionBoxReasonsViewModel(membershipOptionBox);
            DataContext = _viewModel;

            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            Loaded += OnViewLoaded;
        }
        private void OnViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
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
            else if (args.PropertyName == "SetFocusOnOptionBoxReasonText")
            {
                txtOptionBoxReason.CaretIndex = txtOptionBoxReason.Text.Length;
                txtOptionBoxReason.Focus();
            }
        }
        private void RadListBoxOptions_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var originalSender = e.OriginalSource as FrameworkElement;
            if (originalSender != null)
            {
                var item = originalSender.ParentOfType<RadListBoxItem>();
                if (item != null)
                    _viewModel.EditMembershipOptionBoxReasonCommand.Execute(item.DataContext as MembershipOptionBoxReason);
            }
        }
    }
}
