using System.Windows;
using EventManagementSystem.Models;
using Telerik.Windows.Controls;
using EventManagementSystem.ViewModels.Membership;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace EventManagementSystem.Views.Membership
{
    /// <summary>
    /// Interaction logic for EmailView.xaml
    /// </summary>
    public partial class SendEmailView : RadWindow
    {
        private readonly SendEmailViewModel _viewModel;

        public SendEmailView(ObservableCollection<MemberModel> members, CorrespondenceModel correspondence = null)
        {
            InitializeComponent();
            DataContext = _viewModel = new SendEmailViewModel(members, correspondence);

            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            Owner = Application.Current.MainWindow;

            Loaded += OnSendEmailViewLoaded;
            Owner = Application.Current.MainWindow;
        }

        private void OnSendEmailViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.LoadData();
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
            if (args.PropertyName == "CloseDialog")
            {
                DialogResult = true;
                Close();
            }
        }
        private void CancelOnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        //private void PreviewButtonClick(object sender, RoutedEventArgs e)
        //{
        //    _viewModel.IsPreviewButtonClick = true;
        //}
    }
}
