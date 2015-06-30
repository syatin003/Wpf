using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.CRM;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace EventManagementSystem.Views.CRM
{
    /// <summary>
    /// Interaction logic for CRMView.xaml
    /// </summary>
    public partial class CRMView : UserControl
    {
        private readonly CRMViewModel _viewModel;

        public CRMView()
        {
            InitializeComponent();
            DataContext = _viewModel = new CRMViewModel();

            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            Loaded += OnCRMViewLoaded;

            EnquiriesGridView.MouseDoubleClick += EnquiriesGridViewOnMouseDoubleClick;
            ActivitiesRadGridView.MouseDoubleClick += ActivitiesGridViewOnMouseDoubleClick;
            FollowUpsRadGridView.MouseDoubleClick += FollowUpsGridViewOnMouseDoubleClick;
            CampaignsRadGridView.MouseDoubleClick += CampaignsGridViewOnMouseDoubleClick;
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            var tile = this.ParentOfType<RadTileView>();

            if (args.PropertyName == "EnableParentWindow")
            {
                tile.IsEnabled = true;
            }
            else if (args.PropertyName == "DisableParentWindow")
            {
                tile.IsEnabled = false;
            }
        }

        private void OnCRMViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.LoadData();
        }

        private void EnquiriesGridViewOnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var originalSender = e.OriginalSource as FrameworkElement;
            if (originalSender != null)
            {
                var row = originalSender.ParentOfType<GridViewRow>();
                if (row != null)
                {
                    _viewModel.EditEnquiryCommand.Execute(row.Item as EnquiryModel);
                }
            }
        }

        private void ActivitiesGridViewOnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var originalSender = e.OriginalSource as FrameworkElement;
            if (originalSender != null)
            {
                var row = originalSender.ParentOfType<GridViewRow>();
                if (row != null)
                {
                    _viewModel.EditActivityCommand.Execute(row.Item as ActivityModel);
                }
            }
        }

        private void FollowUpsGridViewOnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var originalSender = e.OriginalSource as FrameworkElement;
            if (originalSender != null)
            {
                var row = originalSender.ParentOfType<GridViewRow>();
                if (row != null)
                {
                    _viewModel.EditFollowUpCommand.Execute(row.Item as FollowUpModel);
                }
            }
        }

        private void CampaignsGridViewOnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var originalSender = e.OriginalSource as FrameworkElement;
            if (originalSender != null)
            {
                var row = originalSender.ParentOfType<GridViewRow>();
                if (row != null)
                {
                    _viewModel.EditCampaignCommand.Execute(row.Item as CampaignModel);
                }
            }
        }
    }
}
