using EventManagementSystem.Models;
using EventManagementSystem.Services;
using EventManagementSystem.ViewModels.Membership;
using System;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Navigation;

namespace EventManagementSystem.Views.Membership
{
    /// <summary>
    /// Interaction logic for SendEmailPreView.xaml
    /// </summary>
    public partial class SendEmailPreView : RadWindow
    {
        private readonly SendEmailPreViewModel _viewModel;

        public SendEmailPreView(CorrespondenceModel correspondence, string message, string mainTemplate = null)
        {
            InitializeComponent();
            DataContext = _viewModel = new SendEmailPreViewModel(correspondence, message, mainTemplate);
            RadWindowInteropHelper.SetAllowTransparency(this, false);
            Loaded += SendEmailPreView_Loaded;
            Owner = Application.Current.MainWindow;
        }

        private void SendEmailPreView_Loaded(object sender, RoutedEventArgs e)
        {
           _browser.NavigateToString(_viewModel.Message);
        }
    }
}
