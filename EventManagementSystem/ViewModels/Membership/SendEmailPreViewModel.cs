using EventManagementSystem.Core.Unity;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using System.Windows.Media;
using EventManagementSystem.Services;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Drawing;
using System.Drawing.Imaging;

namespace EventManagementSystem.ViewModels.Membership
{
    public class SendEmailPreViewModel : ViewModelBase
    {
        #region Fields

        private bool _isBusy;
        private String _message;

        #endregion

        #region Properties

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy == value) return;
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        public String Message
        {
            get { return _message; }
            set
            {
                if (_message == value) return;
                _message = value;
                RaisePropertyChanged(() => Message);
            }
        }

        #endregion

        #region Constructor

        public SendEmailPreViewModel(CorrespondenceModel correspondence, string message, string mainTemplate)
        {
            if (mainTemplate != null)
            {
                String messageBody = mainTemplate;
                messageBody = messageBody
                    .Replace("{#EmailHeader}", correspondence.Correspondence.EmailHeader != null && correspondence.Correspondence.EmailHeader.Content != null ? correspondence.Correspondence.EmailHeader.Content : string.Empty)
                    .Replace("{#EmailFooter}", Properties.Settings.Default.CRMEmailFooter)
                    .Replace("{#TemplateColor}", HexColorCodeFromRGB(Properties.Settings.Default.CRMEmailTemplateColor))
                    .Replace("{#Date}", String.Format("{0:dddd dd MMMM}", DateTime.Now))
                    .Replace("{#InternalTemplate}", message);
                var appPath = (string)ApplicationSettings.Read("ImagesPath");
                if (string.IsNullOrWhiteSpace(appPath)) return;
                var fullPathHeaderImage = string.Concat(appPath, correspondence.Correspondence.EmailHeader != null && correspondence.Correspondence.EmailHeader.ImageName != null ? correspondence.Correspondence.EmailHeader.ImageName : string.Empty);
                var fullPathFooterImage = string.Concat(appPath, Properties.Settings.Default.CRMEmailFooterImage);

                if (File.Exists(fullPathHeaderImage))
                {
                    byte[] imageBytes = File.ReadAllBytes(fullPathHeaderImage);
                    messageBody = messageBody.Replace("{#HeaderImage}", fullPathHeaderImage);
                }
                else
                    messageBody = messageBody.Replace("<img src='{#HeaderImage}' border='0' width='580' alt='' />", string.Empty);
                if (File.Exists(fullPathFooterImage))
                {
                    byte[] imageBytes = File.ReadAllBytes(fullPathFooterImage);
                    messageBody = messageBody.Replace("{#FooterImage}", fullPathFooterImage);
                }
                else
                    messageBody = messageBody.Replace("<img src='{#FooterImage}' width='580' alt='' />", string.Empty);

                Message = messageBody;
            }
            else
                Message = message;
        }

        private string HexColorCodeFromRGB(string rgbCode)
        {
            if (rgbCode == string.Empty)
                rgbCode = Colors.Purple.ToString();
            rgbCode = rgbCode.Substring(3);
            return String.Format("#{0}", rgbCode);
        }

        #endregion
    }
}
