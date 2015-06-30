using System.Windows;
using System.Windows.Controls;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Admin.Settings;
using Telerik.Windows.Controls;
using System.ComponentModel;

namespace EventManagementSystem.Views.Admin.Settings
{
    /// <summary>
    /// Interaction logic for TemplatesView.xaml
    /// </summary>
    public partial class TemplatesView : UserControl
    {
        private readonly TemplatesViewModel _viewModel;

        public TemplatesView(MailTemplateCategoryModel category)
        {
            InitializeComponent();
            DataContext = _viewModel = new TemplatesViewModel(category);
            _viewModel.PropertyChanged += ViewModelOnPropertyChanged;

            Loaded += OnMailTemplateViewLoaded;
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

        private void OnMailTemplateViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Loaded -= OnMailTemplateViewLoaded;
            _viewModel.LoadData();
        }
    }
}
