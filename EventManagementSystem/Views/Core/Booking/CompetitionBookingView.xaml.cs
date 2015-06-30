using System.Windows.Controls;
using EventManagementSystem.ViewModels.Core.Booking;

namespace EventManagementSystem.Views.Core.Booking
{
    /// <summary>
    /// Interaction logic for CompetitionBookingView.xaml
    /// </summary>
    public partial class CompetitionBookingView : UserControl
    {
        private readonly CompetitionBookingViewModel _viewModel;

        public CompetitionBookingView()
        {
            InitializeComponent();
            DataContext = _viewModel = new CompetitionBookingViewModel();
        }
    }
}
