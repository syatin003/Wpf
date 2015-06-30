using System.Windows;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.CRM.NewEnquiryTabs.FollowUp;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.CRM.NewEnquiryTabs.FollowUp
{
    /// <summary>
    /// Interaction logic for SnoozeView.xaml
    /// </summary>
    public partial class SnoozeView : RadWindow
    {
        private readonly SnoozeViewModel _viewModel;

        public SnoozeView(FollowUpModel followUp)
        {
            InitializeComponent();
            DataContext = _viewModel = new SnoozeViewModel(followUp);
        }

        private void OnOKButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;

            for (int i = Application.Current.Windows.Count - 1; i >= 0; i--)
            {
                if((Equals(Application.Current.Windows[i].Title, "Follow Up Reminder") || Equals(Application.Current.Windows[i].Title, "Snooze") ))
                {
                     Application.Current.Windows[i].Close();
                }
                //if (!Equals(Application.Current.Windows[i], Application.Current.MainWindow))
                //    Application.Current.Windows[i].Close();
            }
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
