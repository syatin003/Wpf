using System.Windows.Controls;
using EventManagementSystem.ViewModels.Core.Booking;

namespace EventManagementSystem.Views.Core.Booking
{
    /// <summary>
    /// Interaction logic for MatchBookingView.xaml
    /// </summary>
    public partial class MatchBookingView : UserControl
    {
        private readonly MatchBookingViewModel _viewModel;

        public MatchBookingView()
        {
            InitializeComponent();
            DataContext = _viewModel = new MatchBookingViewModel();
        }
    }
}
