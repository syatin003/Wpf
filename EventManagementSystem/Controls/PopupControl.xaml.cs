using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace EventManagementSystem.Controls
{
    /// <summary>
    /// Interaction logic for PopupControl.xaml
    /// </summary>
    public partial class PopupControl : UserControl
    {
        #region Fields

        private readonly SolidColorBrush _successfulColor;
        private readonly SolidColorBrush _failedColor;

        #endregion

        #region Constructors

        public PopupControl()
        {
            InitializeComponent();

            // initialize colors
            var brushes = (ResourceDictionary)Application.LoadComponent(new Uri("Resources/ColorBrushes.xaml", UriKind.Relative));

            _successfulColor = (SolidColorBrush)brushes["EmeraldBrush"];
            _failedColor = (SolidColorBrush)brushes["PomegranateBrush"];
        }

        #endregion

        #region Methods

        public void ShowMessage(string message, MessageType status)
        {
            MessageTextBlock.Text = message;
            RootBorder.Background = (status == MessageType.Successful) ? _successfulColor : _failedColor;
            RootBorder.BringIntoView();
            AnimatePopup();
        }
        public void SetPopUpProperties(double height, double margin, double borderThickness)
        {
            RootBorder.Height = height;
            RootBorder.Margin = new Thickness(margin);
            RootBorder.BorderThickness = new Thickness(borderThickness);
        }
        private void AnimatePopup()
        {
            SetPopUpProperties(40, 5, 1);
            var aminationOpacityShow = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.3));
            var aminationOpacityHide = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.4))
            {
                BeginTime = TimeSpan.FromSeconds(1)
            };

            aminationOpacityShow.Completed += (sender, args) =>
                RootBorder.BeginAnimation(OpacityProperty, aminationOpacityHide);
            aminationOpacityHide.Completed += (sender, args) =>
                SetPopUpProperties(0, 0, 0);

            RootBorder.BeginAnimation(OpacityProperty, aminationOpacityShow);
        }

        #endregion
    }

    public enum MessageType
    {
        Successful,
        Failed
    }
}
