using System;
using System.ComponentModel;
using System.Windows;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Admin.Settings;
using Telerik.Windows.Controls;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Services;

namespace EventManagementSystem.Views.Admin.Settings
{
    /// <summary>
    /// Interaction logic for MailTemplateView.xaml
    /// </summary>
    public partial class MailTemplateView : RadWindow
    {
        public readonly MailTemplateViewModel ViewModel;

        public MailTemplateView(MailTemplateModel model)
        {
            InitializeComponent();
            DataContext = ViewModel = new MailTemplateViewModel(model);

            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;
            Loaded += OnMailTemplateViewLoaded;
        }

        private void OnMailTemplateViewLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Loaded -= OnMailTemplateViewLoaded;
            ViewModel.LoadData();
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "InsertText")
            {
                editor.Insert(ViewModel.Field);
                editor.UpdateEditorLayout();
            }
            else if (args.PropertyName == "InsertImage")
            {
                editor.InsertInline(ViewModel.Image);
                editor.UpdateEditorLayout();
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
