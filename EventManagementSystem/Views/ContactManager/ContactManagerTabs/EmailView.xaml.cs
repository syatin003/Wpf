using System.Windows;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.ContactManager.ContactManagerTabs;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.ContactManager.ContactManagerTabs
{
    /// <summary>
    /// Interaction logic for EmailView.xaml
    /// </summary>
    public partial class EmailView : RadWindow
    {
        private readonly EmailViewModel _viewModel;

         public EmailView(CorrespondenceModel correspondence)
        {
            InitializeComponent();
            DataContext = _viewModel = new EmailViewModel(correspondence);

            Owner = Application.Current.MainWindow;
        }
    }
}
