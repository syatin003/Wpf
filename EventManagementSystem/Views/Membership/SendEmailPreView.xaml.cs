using EventManagementSystem.Models;
using EventManagementSystem.Services;
using EventManagementSystem.ViewModels.Membership;
using System;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Controls;

namespace EventManagementSystem.Views.Membership
{
    /// <summary>
    /// Interaction logic for SendEmailPreView.xaml
    /// </summary>
    public partial class SendEmailPreView : RadWindow
    {
        //private readonly SendEmailPreViewModel _viewModel;

        public SendEmailPreView(CorrespondenceModel correspondence, string mainTemplate = null)
        {
            InitializeComponent();
            //DataContext = _viewModel = new SendEmailPreViewModel(correspondence);

            Owner = Application.Current.MainWindow;
            if (mainTemplate != null)
            {
                String messageBody = mainTemplate;
                messageBody = messageBody
                    .Replace("{#EmailHeader}", correspondence.Correspondence.EmailHeader != null && correspondence.Correspondence.EmailHeader.Content != null ? correspondence.Correspondence.EmailHeader.Content : string.Empty)
                    .Replace("{#EmailFooter}", Properties.Settings.Default.CRMEmailFooter)
                    .Replace("{#TemplateColor}", HexColorCodeFromRGB(Properties.Settings.Default.CRMEmailTemplateColor))
                    //.Replace("{#TemplateColor}", Properties.Settings.Default.CRMEmailTemplateColor != String.Empty ? Properties.Settings.Default.CRMEmailTemplateColor : "#550055")
                    .Replace("{#Date}", String.Format("{0:dddd dd MMMM}", DateTime.Now))
                    .Replace("{#InternalTemplate}", correspondence.Message)
                    .Replace("{#HeaderImage}", "cid:headerImage")
                    .Replace("{#FooterImage}", "cid:footerImage");
                webBrowserEmailPreView.NavigateToString(messageBody);
            }

            else
                webBrowserEmailPreView.NavigateToString(correspondence.Message);
        }

        private string HexColorCodeFromRGB(string rgbCode)
        {
            if (rgbCode == string.Empty)
                rgbCode = Colors.Purple.ToString();
            rgbCode = rgbCode.Substring(3);
            return String.Format("#{0}", rgbCode);
        }

    }
}
