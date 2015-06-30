using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EventManagementSystem.Controls;
using EventManagementSystem.Models;
using Microsoft.Practices.ObjectBuilder2;
using System.Drawing;
using System.Windows.Media;
using Telerik.Windows.Controls;
using EventManagementSystem.Properties;
using System.Windows;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Services
{
    public static class EmailService
    {
        #region Fields

        private static readonly Dictionary<string, string> _mailFields;

        #endregion

        #region Constructor

        static EmailService()
        {
            _mailFields = PrepareEmailFields();
        }

        #endregion

        #region Methods

        private static Dictionary<string, string> PrepareEmailFields()
        {
            var fields = new Dictionary<string, string>
            {
                {"Event Name", "{#EventName}"},
                {"Current Date", "{#CurrentDate}"},
                {"Event Date", "{#EventDate}"},
                {"Creation Date", "{#CreationDate}"},
                {"Num of People", "{#NumofPeople}"},
                {"Type", "{#Type}"},
                {"Status", "{#Status}"},
                {"Value", "{#Value}"},
                {"Value Each", "{#ValueEach}"},
                {"Contact Title", "{#ContactTitle}"},
                {"Contact First Name", "{#ContactFirstName}"},
                {"Contact Last Name", "{#ContactLastName}"},
                {"Contact Full Name", "{#ContactFullName}"},
                {"Contact Address 1", "{#ContactAddress1}"},
                {"Contact Address 2", "{#ContactAddress2}"},
                {"Contact Address 3", "{#ContactAddress3}"},
                {"Contact Town", "{#ContactTown}"},
                {"Contact County", "{#ContactCounty}"},
                {"Contact Post Code", "{#ContactPostCode}"},
                {"Contact Address Block", "{#ContactAddressBlock}"},
                {"Contact Company Name", "{#ContactCompanyName}"},
                {"Contact Phone 1", "{#ContactPhone1}"},
                {"Contact Phone 2", "{#ContactPhone2}"},
                {"Contact Email", "{#ContactEmail}"},
                {"Deposit Amount", "{#DepositAmount}"},
                {"Deposit Date", "{#DepositDate}"},
                {"Deposit Payment Method", "{#DepositPaymentMethod}"},
                {"Payment Amount", "{#PaymentAmount}"},
                {"Payment Date", "{#PaymentDate}"},
                {"Payment Method", "{#PaymentMethod}"},
                {"Items Block", "{#ItemsBlock}"},
                {"Secondary Contact Title", "{#SecondaryContactTitle}"},
                {"Secondary Contact First Name", "{#SecondaryContactFirstName}"},
                {"Secondary Contact Last Name", "{#SecondaryContactLastName}"},
                {"Secondary Contact Phone1", "{#SecondaryContactPhone1}"},
                {"Secondary Contact Phone2", "{#SecondaryContactPhone2}"},
                {"Secondary Contact Email", "{#SecondaryContactEmail}"},
                {"External Notes", "{#ExternalNotes}"},
                {"Internal Notes", "{#InternalNotes}"},
                {"Email Signature", "{#EmailSignature}"},
                {"Member Category", "{#MemberCategory}"},
                {"Start Date", "{#StartDate}"},
                {"Member Reference", "{#MemberReference}"},
                {"Renewal Date", "{#RenewalDate}"},

            };

            return fields;
        }

        public static IEnumerable<string> GetMailFieldsNames()
        {
            return _mailFields.Keys;
        }

        public static string GetMailField(string name)
        {
            return _mailFields[name];
        }

        public static async Task<bool> SendEmail(CorrespondenceModel correspondence, Action onCompleteAction = null, string mainTemplate = null)
        {
            bool result = false;
            try
            {
                var settings = PrepareSettings();

                if (settings.HasErrors)
                {
                    PopupService.ShowMessage("Please check your SMTP settings in Admin module", MessageType.Failed);
                    return false;
                }

                using (var smtpClient = new SmtpClient(settings.Server))
                {
                    smtpClient.Port = 587;
                    smtpClient.EnableSsl = settings.EnableSsl;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(settings.Username, settings.Password);

                    using (var message = new MailMessage())
                    {

                        message.From = new MailAddress(correspondence.FromAddress);
                        message.IsBodyHtml = true;
                        message.Subject = correspondence.Subject;
                        message.SubjectEncoding = Encoding.UTF8;

                        if (mainTemplate != null)
                        {
                            ApplyMainTemplate(correspondence.Message, mainTemplate, message, correspondence.Correspondence.EmailHeader);
                        }
                        else
                        {
                            message.Body = correspondence.Message;
                        }
                        message.BodyEncoding = Encoding.UTF8;
                        message.To.Add(correspondence.ToAddress);

                        PrepareCcContacts(correspondence, message);
                        bool isSuccess = PrepareAttachments(correspondence, message);

                        if (isSuccess)
                        {
                            await smtpClient.SendMailAsync(message);

                            PopupService.ShowMessage("Email was sent", MessageType.Successful);

                            if (onCompleteAction != null)
                                onCompleteAction.Invoke();
                            result = true;
                        }
                        else
                            result = false;
                    }
                }
            }
            catch (Exception ex)
            {
                PopupService.ShowMessage(ex.Message, MessageType.Failed);
            }
            return result;
        }

        private static void ApplyMainTemplate(String correspondenceMessage, String mainTemplate, MailMessage message, EmailHeader emailHeader)
        {
            String messageBody = mainTemplate;
            messageBody = messageBody
                //.Replace("{#EmailHeader}", Properties.Settings.Default.CRMEmailHeader)
                .Replace("{#EmailHeader}", emailHeader != null && emailHeader.Content != null ? emailHeader.Content : string.Empty)
                .Replace("{#EmailFooter}", Properties.Settings.Default.CRMEmailFooter)
                .Replace("{#TemplateColor}", HexColorCodeFromRGB(Properties.Settings.Default.CRMEmailTemplateColor))
                //.Replace("{#TemplateColor}", Properties.Settings.Default.CRMEmailTemplateColor != String.Empty ? Properties.Settings.Default.CRMEmailTemplateColor : "#550055")
                .Replace("{#Date}", String.Format("{0:dddd dd MMMM}", DateTime.Now))
                .Replace("{#InternalTemplate}", correspondenceMessage)
                .Replace("{#HeaderImage}", "cid:headerImage")
                .Replace("{#FooterImage}", "cid:footerImage");

            var appPath = (string)ApplicationSettings.Read("ImagesPath");
            if (string.IsNullOrWhiteSpace(appPath)) return;
            //var fullPathHeaderImage = string.Concat(appPath, Properties.Settings.Default.CRMEmailHeaderImage);
            var fullPathHeaderImage = string.Concat(appPath, emailHeader != null && emailHeader.ImageName != null ? emailHeader.ImageName : string.Empty);
            var fullPathFooterImage = string.Concat(appPath, Properties.Settings.Default.CRMEmailFooterImage);
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(messageBody, Encoding.UTF8, "text/html");
            if (File.Exists(fullPathHeaderImage))
            {
                LinkedResource headerImage = new LinkedResource(fullPathHeaderImage, MediaTypeNames.Image.Jpeg);
                headerImage.ContentId = "headerImage";
                htmlView.LinkedResources.Add(headerImage);
            }
            if (File.Exists(fullPathFooterImage))
            {
                LinkedResource footerImage = new LinkedResource(fullPathFooterImage, MediaTypeNames.Image.Jpeg);
                footerImage.ContentId = "footerImage";
                htmlView.LinkedResources.Add(footerImage);
            }
            message.AlternateViews.Add(htmlView);
        }

        private static void PrepareCcContacts(CorrespondenceModel correspondence, MailMessage message)
        {
            if (correspondence.SendMailToCcAddress && !string.IsNullOrWhiteSpace(correspondence.CCAddress))
            {
                string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                                 + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                                 + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

                var regex = new Regex(pattern, RegexOptions.IgnoreCase);

                string[] emails = correspondence.CCAddress.Split(',');

                if (emails.Any())
                    emails.Where(email => !string.IsNullOrWhiteSpace(email) && regex.IsMatch(email))
                        .ForEach(message.To.Add);
            }
        }

        private static bool PrepareAttachments(CorrespondenceModel model, MailMessage message)
        {
            var appPath = (string)ApplicationSettings.Read("DocumentsPath");

            if (string.IsNullOrWhiteSpace(appPath) || !model.Documents.Any()) return true;

            var response = false;
            foreach (var document in model.Documents)
            {
                var fullPath = string.Concat(appPath, document.Path);

                if (File.Exists(fullPath))
                {
                    var attachment = new Attachment(fullPath, MediaTypeNames.Application.Octet);
                    ContentDisposition disposition = attachment.ContentDisposition;
                    disposition.CreationDate = File.GetCreationTime(fullPath);
                    disposition.ModificationDate = File.GetLastWriteTime(fullPath);
                    disposition.ReadDate = File.GetLastAccessTime(fullPath);
                    disposition.FileName = Path.GetFileName(fullPath);
                    disposition.Size = new FileInfo(fullPath).Length;
                    disposition.DispositionType = DispositionTypeNames.Attachment;
                    message.Attachments.Add(attachment);
                    response = true;
                }
                else
                {
                    RadWindow.Alert(Resources.MESSAGE_DOCUMENT_NOT_FOUND);
                    response = false;
                }
            }
            return response;
        }

        private static EmailServerSettingsModel PrepareSettings()
        {
            var settings = new EmailServerSettingsModel()
            {
                Server = (string)ApplicationSettings.Read("Server"),
                Username = (string)ApplicationSettings.Read("Username"),
                Password = (string)ApplicationSettings.Read("Password"),
            };

            var enableSsl = ApplicationSettings.Read("EnableSsl");

            if (enableSsl != null)
                settings.EnableSsl = Convert.ToBoolean(enableSsl);

            var port = ApplicationSettings.Read("Port");

            if (port != null)
                settings.Port = Convert.ToInt32(port);

            return settings;
        }

        private static string HexColorCodeFromRGB(string rgbCode)
        {
            if (rgbCode == string.Empty)
                rgbCode = Colors.Purple.ToString();
            rgbCode = rgbCode.Substring(3);
            return String.Format("#{0}", rgbCode);
        }

        #endregion
    }
}