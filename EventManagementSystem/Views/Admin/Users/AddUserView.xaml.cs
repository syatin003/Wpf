using System;
using System.Windows;
using EventManagementSystem.ViewModels.Admin.Users;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Admin.Users
{
    /// <summary>
    /// Interaction logic for AddUserView.xaml
    /// </summary>
    public partial class AddUserView : RadWindow
    {
        public AddUserViewModel ViewModel { get; private set; }

        public AddUserView()
        {
            InitializeComponent();
            DataContext = ViewModel = new AddUserViewModel();

            Owner = Application.Current.MainWindow;

            Loaded += OnAddUserViewLoaded;
        }

        private void OnAddUserViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            ViewModel.LoadData();
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
