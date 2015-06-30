using EventManagementSystem.Data.Model;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Admin.Settings;
using System.ComponentModel;
using System.Windows;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Admin.Settings
{
    /// <summary>
    /// Interaction logic for AddEmailHeaderView.xaml
    /// </summary>
    public partial class AddEmailHeaderView : RadWindow
    {
        public readonly AddEmailHeaderViewModel ViewModel;

        public AddEmailHeaderView(EmailHeaderModel emailHeader)
        {
            InitializeComponent();
            DataContext = ViewModel = new AddEmailHeaderViewModel(emailHeader);
            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;

            Owner = Application.Current.MainWindow;
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "EnableParentWindow")
            {
                IsEnabled = true;
            }
            else if (args.PropertyName == "DisableParentWindow")
            {
                IsEnabled = false;
            }
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

    }
}
