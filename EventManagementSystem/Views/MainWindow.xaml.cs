using System.ComponentModel;
using System.Windows;
using EventManagementSystem.Controls;
using EventManagementSystem.ViewModels;
using Telerik.Windows.Controls;
using System.Windows.Media;
using System.Windows.Controls;
using EventManagementSystem.Views.Core;

namespace EventManagementSystem.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RadRibbonWindow
    {
        public MainWindowModel ViewModel { get; private set; }

        static MainWindow()
        {
            RadRibbonWindow.IsWindowsThemeEnabled = false;
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = ViewModel = new MainWindowModel();

#if (!DEBUG)
            Closing += MainWindowOnClosing;
#endif
        }

        private void MainWindowOnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            bool? dialogResult = null;
            string confirmText = Properties.Resources.MESSAGE_ASK_BEFORE_CLOSE_APPLICATION;

            ViewModel.ChangePopupOverlay();
            RadWindow.Confirm(new DialogParameters()
            {
                Owner = Application.Current.MainWindow,
                Content = confirmText,
                Closed = (s, args) => { dialogResult = args.DialogResult; }
            });
            ViewModel.ChangePopupOverlay();

            if (dialogResult != true)
                cancelEventArgs.Cancel = true;
        }

        public void ShowMessage(string message, MessageType status)
        {
            PopupControl.ShowMessage(message, status);
        }
    }
}