using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using EventManagementSystem.Controls;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.UnitOfWork;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Helpers;
using EventManagementSystem.Services;
using Microsoft.Practices.Unity;
using Telerik.Windows.Controls;

namespace EventManagementSystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            DispatcherUnhandledException += OnDispatcherUnhandledException;

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

            InitializeUnityContainer();
            InitializeColors();
            InitializeReportsFolder();
            InitializeImagesFolder();
        }

        private void InitializeUnityContainer()
        {
            IUnityContainer unityContainer = ContainerAccessor.Instance.GetContainer();
            unityContainer.RegisterType<IDataUnitLocator, DataUnitLocator>(new ContainerControlledLifetimeManager());
        }

        private static void InitializeColors()
        {
            var brushes = (ResourceDictionary)Application.LoadComponent(new Uri("Resources/ColorBrushes.xaml", UriKind.Relative));
            var solidColorBrush = (SolidColorBrush)brushes["BelizeHoleBrush"];

            Windows8Palette.Palette.AccentColor = solidColorBrush.Color;

            StyleManager.ApplicationTheme = new Windows8Theme();
        }

        private void InitializeReportsFolder()
        {
            var appPath = (string)ApplicationSettings.Read("DocumentsPath");

            if (string.IsNullOrWhiteSpace(appPath) || !Directory.Exists(appPath))
            {
                appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\documents\";

                if (!Directory.Exists(appPath))
                    Directory.CreateDirectory(appPath);

                ApplicationSettings.Write("DocumentsPath", appPath);
            }
        }
        private void InitializeImagesFolder()
        {
            var appPath = (string)ApplicationSettings.Read("ImagesPath");

            if (string.IsNullOrWhiteSpace(appPath) || !Directory.Exists(appPath))
            {
                appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\Images\";

                if (!Directory.Exists(appPath))
                    Directory.CreateDirectory(appPath);

                ApplicationSettings.Write("ImagesPath", appPath);
            }
        }
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args)
        {
            PopupService.ShowMessage(args.Exception.Message, MessageType.Failed);

#if (!DEBUG)
            args.Handled = true;
#endif
        }
    }
}