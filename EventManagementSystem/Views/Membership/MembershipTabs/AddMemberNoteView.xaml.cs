using System.Windows;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Membership.MembershipTabs;

namespace EventManagementSystem.Views.Membership.MembershipTabs
{
    /// <summary>
    /// Interaction logic for AddMemberNoteView.xaml
    /// </summary>
    public partial class AddMemberNoteView
    {
        public readonly AddMemberNoteViewModel ViewModel;

        public AddMemberNoteView(MemberModel memberModel, MemberNoteModel memberNote = null)
        {
            InitializeComponent();
            DataContext = ViewModel = new AddMemberNoteViewModel(memberModel, memberNote);

            Owner = Application.Current.MainWindow;
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
