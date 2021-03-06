﻿using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventManagementSystem.Models;
using EventManagementSystem.Data.Model;
using System.Collections.ObjectModel;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Core.Unity;
using Microsoft.Practices.Unity;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Services;
using EventManagementSystem.Properties;
using System.IO;
using System.ComponentModel;
using Microsoft.Practices.ObjectBuilder2;
using EventManagementSystem.Views.Membership;
using System.Text.RegularExpressions;
using Telerik.Windows.Controls;
using System.Windows;
using System.Windows.Controls;

namespace EventManagementSystem.ViewModels.Membership
{
    public class SendEmailViewModel : ViewModelBase
    {
        #region Fields

        private readonly IMembershipDataUnit _membershipDataUnit;
        private bool _isBusy;
        private CorrespondenceModel _correspondence;
        private List<EmailHeader> _allEmailHeaders;
        private ObservableCollection<EmailHeader> _emailHeaders;
        private ObservableCollection<MailTemplateModel> _mailTemplates;
        private MailTemplateModel _selectedMailTemplate;
        private MailTemplateModel _mainEmailTemplate;
        private CorresponcenceType _corresponcenceType;
        private bool _isResend;
        private ObservableCollection<MemberModel> _members;
        private ObservableCollection<MemberModel> _membersHavingValidEmail;
        private ObservableCollection<MemberModel> _membersHavingInvalidEmail;
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

        public CorrespondenceModel Correspondence
        {
            get { return _correspondence; }
            set
            {
                if (_correspondence == value) return;
                _correspondence = value;
                RaisePropertyChanged(() => Correspondence);
            }
        }

        public ObservableCollection<MailTemplateModel> MailTemplates
        {
            get { return _mailTemplates; }
            set
            {
                if (_mailTemplates == value) return;
                _mailTemplates = value;
                RaisePropertyChanged(() => MailTemplates);
            }
        }

        public ObservableCollection<EmailHeader> EmailHeaders
        {
            get { return _emailHeaders; }
            set
            {
                if (_emailHeaders == value) return;
                _emailHeaders = value;
                RaisePropertyChanged(() => EmailHeaders);
            }
        }

        public MailTemplateModel SelectedMailTemplate
        {
            get { return _selectedMailTemplate; }
            set
            {
                if (_selectedMailTemplate == value) return;
                _selectedMailTemplate = value;
                Message = _selectedMailTemplate.MailTemplate.Template;
                RaisePropertyChanged(() => SelectedMailTemplate);
                Correspondence.Subject = _selectedMailTemplate.Name;
            }
        }

        public MailTemplateModel MainEmailTemplate
        {
            get { return _mainEmailTemplate; }
            set
            {
                if (_mainEmailTemplate == value) return;
                _mainEmailTemplate = value;
            }
        }

        public ObservableCollection<MemberModel> MembersHavingValidEmail
        {
            get { return _membersHavingValidEmail; }
            set
            {
                if (_membersHavingValidEmail == value) return;
                _membersHavingValidEmail = value;
                RaisePropertyChanged(() => MembersHavingValidEmail);
            }
        }

        public ObservableCollection<MemberModel> MembersHavingInvalidEmail
        {
            get { return _membersHavingInvalidEmail; }
            set
            {
                if (_membersHavingInvalidEmail == value) return;
                _membersHavingInvalidEmail = value;
                RaisePropertyChanged(() => MembersHavingInvalidEmail);
            }
        }

        public ObservableCollection<MemberModel> Members
        {
            get { return _members; }
            set
            {
                if (_members == value) return;
                _members = value;
                RaisePropertyChanged(() => Members);
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

        public RelayCommand PreviewCommand { get; private set; }
        public RelayCommand SubmitCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        #endregion

        #region Constructor

        public SendEmailViewModel(ObservableCollection<MemberModel> members, CorrespondenceModel correspondence)
        {
            Members = members;

            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            var regex = new Regex(pattern, RegexOptions.IgnoreCase);

            MembersHavingValidEmail = new ObservableCollection<MemberModel>(members.Where(member => !string.IsNullOrWhiteSpace(member.Contact.Email) && regex.IsMatch(member.Contact.Email)));

            MembersHavingInvalidEmail = new ObservableCollection<MemberModel>(members.Where(member => string.IsNullOrWhiteSpace(member.Contact.Email) || !regex.IsMatch(member.Contact.Email)));

            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _membershipDataUnit = dataUnitLocator.ResolveDataUnit<IMembershipDataUnit>();

            PreviewCommand = new RelayCommand(PreviewCommandExecuted, PreviewCommandCanExecute);
            SubmitCommand = new RelayCommand(SubmitCommandExecuted, SubmitCommandCanExecute);
            CancelCommand = new RelayCommand(CancelCommandExecuted);
            ProcessEmail(correspondence);
        }

        #endregion

        #region Methods

        public async void LoadData()
        {
            IsBusy = true;
            _membershipDataUnit.MailTemplatesRepository.Refresh();
            var templates = await _membershipDataUnit.MailTemplatesRepository.GetAllAsync(x => x.MailTemplateCategory.Name == "Membership");

            MailTemplates = new ObservableCollection<MailTemplateModel>(templates.Where(x => x.IsEnabled && x.MailTemplateType.Name != "MainEmailTemplate").OrderBy(x => x.Name).Select(x => new MailTemplateModel(x)));

            MainEmailTemplate = new MailTemplateModel(templates.Where(x => x.MailTemplateType.Name == "MainEmailTemplate").FirstOrDefault());

            _membershipDataUnit.EmailHeadersRepository.Refresh();
            var headers = await _membershipDataUnit.EmailHeadersRepository.GetAllAsync();
            _allEmailHeaders = new List<EmailHeader>(headers);
            EmailHeaders = new ObservableCollection<EmailHeader>(_allEmailHeaders.Where(x => x.IsEnabled));

            var types = await _membershipDataUnit.CorresponcenceTypesRepository.GetAllAsync(x => x.Type == "Member");
            _corresponcenceType = types.FirstOrDefault();

            IsBusy = false;
        }

        private void ProcessEmail(CorrespondenceModel mail)
        {
            _isResend = (mail != null);

            Correspondence = (_isResend) ? CopyCorrespondence(mail) : GetCorrespondence();
            Correspondence.PropertyChanged += Correspondence_PropertyChanged;
            this.PropertyChanged += SendEmailViewModel_PropertyChanged;
        }

        private void Correspondence_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SubmitCommand.RaiseCanExecuteChanged();
            PreviewCommand.RaiseCanExecuteChanged();
        }

        private void SendEmailViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedMailTemplate")
            {
                if (_selectedMailTemplate.EmailHeader != null)
                {
                    _correspondence.EmailHeader = _selectedMailTemplate.EmailHeader;
                    EmailHeaders = new ObservableCollection<EmailHeader>(_allEmailHeaders.Where(x => x.IsEnabled || x.ID == _selectedMailTemplate.EmailHeader.ID));
                }
                else
                    EmailHeaders = new ObservableCollection<EmailHeader>(_allEmailHeaders.Where(x => x.IsEnabled));
            }
        }

        private CorrespondenceModel GetCorrespondence()
        {
            Message = "";
            var model = new CorrespondenceModel(new Corresponcence()
            {
                ID = Guid.NewGuid(),
                Date = DateTime.Now,
                OwnerType = "Membership",
                EmailType = "Email",
                Message = "",
                FromAddress = AccessService.Current.User.EmailAddress,
                ToAddress = MembersHavingValidEmail.Select(member => member.Contact.Email).FirstOrDefault(),
                UserID = AccessService.Current.User.ID
            });

            return model;
        }

        private CorrespondenceModel CopyCorrespondence(CorrespondenceModel correspondence)
        {
            var model = new CorrespondenceModel(new Corresponcence()
            {
                ID = Guid.NewGuid(),
                Date = DateTime.Now,
                OwnerType = "Membership",
                EmailType = "Email",
                UserID = AccessService.Current.User.ID
            })
            {
                FromAddress = correspondence.FromAddress,
                ToAddress = correspondence.ToAddress,
                Message = correspondence.Message,
                Subject = correspondence.Subject,
                EmailType = correspondence.EmailType,
                CorresponcenceType = correspondence.CorresponcenceType
            };

            return model;
        }

        private void AddCorrespondenceOnMembersRecord()
        {
            foreach (var memberContact in MembersHavingValidEmail)
            {
                _membershipDataUnit.CorresponcencesRepository.Add(GetCorrespondenceByMember(memberContact));
            }
        }

        private Corresponcence GetCorrespondenceByMember(MemberModel member)
        {
            return new Corresponcence()
            {
                ID = Guid.NewGuid(),
                Date = DateTime.Now,
                OwnerID = member.Member.ID,
                OwnerType = "Membership",
                EmailType = "Email",
                Message = _selectedMailTemplate != null ? ParseMailMessage(member) : Message,
                Subject = Correspondence.Subject,
                FromAddress = Correspondence.FromAddress,
                ToAddress = member.Contact.Email,
                UserID = AccessService.Current.User.ID,
                CorresponcenceType = Correspondence.CorresponcenceType,
                EmailHeader = Correspondence.EmailHeader
            };
        }

        private string ParseMailMessage(MemberModel member)
        {
            var inputText = Message;

            string outputText;

            outputText = inputText;

            if (member.Contact.Title != null)
            {
                outputText = outputText
                    .Replace("{#ContactTitle}", member.Contact.Title.Title);
            }
            outputText = outputText
                .Replace("{#ContactFirstName}", member.Contact.FirstName)
                .Replace("{#ContactLastName}", member.Contact.LastName);

            outputText = outputText
                .Replace("{#MemberCategory}", member.Category.Name)
                .Replace("{#StartDate}", Convert.ToDateTime(member.StartDate).ToString("d"))
                .Replace("{#MemberReference}", member.Member.MemberReference)
                .Replace("{#RenewalDate}", Convert.ToDateTime(member.RenewalDate).ToString("d"))

                .Replace("{#EmailSignature}", AccessService.Current.User.EmailSignature);

            return outputText;
            //Correspondence.Message = outputText;
            //Correspondence.Subject = _selectedMailTemplate.Name;
        }

        #endregion

        #region Commands

        private void PreviewCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");
            string message = "";
            if (_selectedMailTemplate != null)
                message = ParseMailMessage(MembersHavingValidEmail.FirstOrDefault());
            else
                message = Message;
            var window = new SendEmailPreView(Correspondence, message, MainEmailTemplate.MailTemplate.Template);
            window.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");
        }

        private bool PreviewCommandCanExecute()
        {
            return !Correspondence.HasErrors;
        }

        private async void SubmitCommandExecuted()
        {
            IsBusy = true;
            if (!_isResend)
            {
                Correspondence.CorresponcenceType = _corresponcenceType;
            }
             //   if (Correspondence.SaveOnClientsRecord)
            //    AddCorrespondenceOnMembersRecord();
          
            var membersEmailNotSent = new List<MemberModel>();

            foreach (var member in MembersHavingValidEmail)
            {
                var newMemberCorrespondence = new CorrespondenceModel(GetCorrespondenceByMember(member));
                bool success = await EmailService.SendEmail(newMemberCorrespondence, MainEmailTemplate.MailTemplate.Template);
                if (success)
                {
                    // add entry into update log
                    var update = new MembershipUpdate()
                    {
                        ID = Guid.NewGuid(),
                        MemberID = member.Member.ID,
                        Date = DateTime.Now,
                        Message = string.Format("Email was sent to {0}", member.Contact.Email),
                        UserID = AccessService.Current.User.ID,
                        OldValue = null,
                        NewValue = member.Contact.Email,
                        ItemId = newMemberCorrespondence.Correspondence.ID,
                        ItemType = "Correspondence",
                        Field = "Email",
                        Action = Convert.ToInt32(UpdateAction.Added)
                    };
                    // If false don't save Correspondence.
                    // Make ItemId = null;
                    if (!Correspondence.SaveOnClientsRecord)
                    {
                        update.ItemId = null;
                    }
                    _membershipDataUnit.MembershipUpdatesRepository.Add(update);
                }
                else
                {
                    membersEmailNotSent.Add(member);
                }
            }
            _membershipDataUnit.CorresponcencesRepository.RevertChanges(Correspondence.SaveOnClientsRecord);
           await _membershipDataUnit.SaveChanges();

            IsBusy = false;

            string confirmText = Members.Count + " members selected";

            confirmText = MembersHavingInvalidEmail.Count > 0 ?
                (confirmText + ", " + MembersHavingInvalidEmail.Count + " have no email addresses") : confirmText;

            confirmText = MembersHavingValidEmail.Except(membersEmailNotSent).Count() > 0 ? (confirmText + ", " + MembersHavingValidEmail.Except(membersEmailNotSent).Count() + " sent successfully") :

          confirmText = membersEmailNotSent.Count > 0 ? (confirmText + ", " + membersEmailNotSent.Count + " not sent due to error having following names :- " +
          
          string.Join("\n", membersEmailNotSent.Select(x => x.Contact.FirstName).ToArray())) : confirmText;

            RaisePropertyChanged("DisableParentWindow");

            RadWindow.Alert(new DialogParameters
                {
                    Owner = Application.Current.MainWindow,
                    Content = new TextBlock() { Text = confirmText, TextWrapping = TextWrapping.Wrap, Width = 300 }
                });

            RaisePropertyChanged("EnableParentWindow");

            RaisePropertyChanged("CloseDialog");
        }

        private bool SubmitCommandCanExecute()
        {
            return !Correspondence.HasErrors;
        }

        private void CancelCommandExecuted()
        {
            _membershipDataUnit.RevertChanges();
        }

        #endregion
    }
}