using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Membership.MembershipTabs;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace EventManagementSystem.Views.Membership.MembershipTabs
{
    /// <summary>
    /// Interaction logic for MemberNotesView.xaml
    /// </summary>
    public partial class MemberNotesView
    {
        private readonly MemberNotesViewModel _viewModel;

        public MemberNotesView(MemberModel member)
        {
            InitializeComponent();
            DataContext = _viewModel = new MemberNotesViewModel(member);
            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            RadGridViewNotes.MouseDoubleClick += RadGridViewNotes_MouseDoubleClick;

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

        private void RadGridViewNotes_MouseDoubleClick(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            var originalSender = mouseButtonEventArgs.OriginalSource as FrameworkElement;
            if (originalSender != null)
            {
                var row = originalSender.ParentOfType<GridViewRow>();
                if (row != null)
                {
                    _viewModel.EditNoteCommand.Execute(row.Item as MemberNoteModel);
                }
            }
        }
    }
}
