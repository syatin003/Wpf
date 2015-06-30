using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using EventManagementSystem.ViewModels.Admin.Resources;
using EventManagementSystem.Models;
using Telerik.Windows.Controls;
using System.ComponentModel;

namespace EventManagementSystem.Views.Admin.Resources
{
    /// <summary>
    /// Interaction logic for FollowResource.xaml
    /// </summary>
    public partial class GolfFollowResource : RadWindow
    {

        public readonly GolfFollowResourceViewModel ViewModel;

        public GolfFollowResource(GolfModel golfModel)
        {
            InitializeComponent();
            DataContext = ViewModel = new GolfFollowResourceViewModel(golfModel);

            Owner = Application.Current.MainWindow;
            Loaded += OnGolfFollowResourcesViewLoaded;
            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "AddSelectedItems")
            {
                FollowGolfListBox.SelectedItems.AddRange(ViewModel.GolfFollowResources);
            }
        }


        private void OnGolfFollowResourcesViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            ViewModel.LoadData();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void SubmitButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void FollowGolfListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SetGolfFollowResources(FollowGolfListBox.SelectedItems);
        }
    }
}
