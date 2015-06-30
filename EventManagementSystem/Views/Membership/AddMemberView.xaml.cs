using System.Windows;
using System.Windows.Input;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Membership;
using Telerik.Windows.Controls;
using System.ComponentModel;

namespace EventManagementSystem.Views.Membership
{
    /// <summary>
    /// Interaction logic for AddMemberView.xaml
    /// </summary>
    public partial class AddMemberView : RadWindow
    {
        public readonly AddMemberViewModel ViewModel;

        public AddMemberView(MemberModel member = null)
        {
            InitializeComponent();
            DataContext = ViewModel = new AddMemberViewModel(member);
            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;

            Owner = Application.Current.MainWindow;

            Loaded += OnAddMemberViewLoaded;
        }

        private void OnAddMemberViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            ViewModel.LoadData();
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "CloseDialog")
            {
                DialogResult = true;
                Close();
            }
            if (args.PropertyName == "EnableParentWindow")
            {
                IsEnabled = true;
            }
            else if (args.PropertyName == "DisableParentWindow")
            {
                IsEnabled = false;
            }
        }

        private async void TxtLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.IsOkButtonClick)
            {
                if (TxtLastName.Text != string.Empty && TxtEmail.Text != string.Empty)
                {
                    if (!ViewModel.IsIgnored)
                        await ViewModel.ShowReleventContact();
                }
            }
        }

        private async void TxtEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.IsOkButtonClick)
            {
                if (TxtEmail.Text != string.Empty && TxtLastName.Text != string.Empty)
                {
                    if (!ViewModel.IsIgnored)
                        await ViewModel.ShowReleventContact();
                }
            }
        }
    }
}
