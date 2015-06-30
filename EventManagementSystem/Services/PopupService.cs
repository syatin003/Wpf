using System.Windows;
using EventManagementSystem.Controls;
using EventManagementSystem.Views;

namespace EventManagementSystem.Services
{
    public class PopupService
    {
        #region Fields

        private static readonly MainWindow _mainWindow;

        #endregion

        #region Constructors

        static PopupService()
        {
            _mainWindow = (MainWindow)Application.Current.MainWindow;
        }

        #endregion

        #region Messages

        public static void ShowMessage(string message, MessageType status)
        {
            _mainWindow.ShowMessage(message, status);
        }

        #endregion
    }
}
