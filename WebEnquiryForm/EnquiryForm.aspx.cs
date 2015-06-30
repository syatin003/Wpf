using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using Microsoft.Practices.Unity;

namespace WebEnquiryForm
{
    public partial class EnquiryForm : System.Web.UI.Page
    {
        private readonly IWebEnquiryDataUnit _webEnquiryDataUnit;

        public ObservableCollection<EventType> EventTypes { get; set; }

        public string EventDetails { get; set; }

        private void InitializeUnityContainer()
        {
            IUnityContainer unityContainer = ContainerAccessor.Instance.GetContainer();
            unityContainer.RegisterType<IDataUnitLocator, DataUnitLocator>(new ContainerControlledLifetimeManager());
        }

        public EnquiryForm()
        {
            InitializeUnityContainer();

            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _webEnquiryDataUnit = dataUnitLocator.ResolveDataUnit<IWebEnquiryDataUnit>();
        }

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["CheckRefresh"] =
                Server.UrlDecode(DateTime.Now.ToString());

                EventTypes = new ObservableCollection<EventType>(await _webEnquiryDataUnit.EventTypesRepository.GetAllAsync());
                EventTypesDropDown.DataSource = EventTypes;
                EventTypesDropDown.DataTextField = "Name";
                EventTypesDropDown.DataValueField = "ID";
                EventTypesDropDown.DataBind();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ViewState["CheckRefresh"] = Session["CheckRefresh"];
        }

        protected async void OnClick(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            if (Session["CheckRefresh"].ToString() == ViewState["CheckRefresh"].ToString())
            {
                Session["CheckRefresh"] = Server.UrlDecode(DateTime.Now.ToString());
            }
            else
            {
                ClearAll();
                return;
            }

            DefaultSettingsForEnquiry defaultSettings =
                (await _webEnquiryDataUnit.DefaultSettingsForEnquiriesRepository.GetAllAsync()).First();

            var user = (await _webEnquiryDataUnit.UsersRepository.GetUsersAsync(x => x.ID == defaultSettings.UserID)).FirstOrDefault();

            var emailSettings = (await _webEnquiryDataUnit.EmailSettingsRepository.GetAllAsync()).First();

            var receivedMethods =
                new List<EnquiryReceiveMethod>(await _webEnquiryDataUnit.EnquiryReceiveMethodsRepository.GetAllAsync());
            var eventStatuses = new List<EventStatus>(await _webEnquiryDataUnit.EventStatusesRepository.GetAllAsync());

            var places = String.IsNullOrEmpty(txtNum.Text) ? 0 : Convert.ToInt32(txtNum.Text);
            var name = String.IsNullOrEmpty(txtName.Text) ? "Web Enquiry" : txtName.Text;
            DateTime? date;
            if (!String.IsNullOrEmpty(txtDate.Text))
                date = DateTime.ParseExact(txtDate.Text, "dd/MM/yyyy", CultureInfo.CurrentCulture);
            else
            {
                date = null;
            }

            var contact = new Contact()
            {
                ID = Guid.NewGuid(),
                FirstName = txtFirstName.Text,
                LastName = txtLastName.Text,
                Phone1 = txtMobilePhone.Text,
                Email = txtEmail.Text
            };
            _webEnquiryDataUnit.ContactsRepository.Add(contact);

            var enquiry = new Enquiry()
            {
                ID = Guid.NewGuid(),
                Date = date,
                Name = name,
                EventTypeID = Guid.Parse(EventTypesDropDown.SelectedValue),
                EventStatusID = eventStatuses.First(x => x.Name.Equals("Enquiry")).ID,
                Places = places,
                ContactID = contact.ID,
                EnquiryStatusID = defaultSettings.EnquiryStatusID,
                AssignedToID = defaultSettings.UserID,
                ReceivedMethodID = receivedMethods.First(x => x.ReceiveMethod == "web site").ID,
                TakenByID = defaultSettings.UserID,
                CreationDate = DateTime.Now
            };
            _webEnquiryDataUnit.EnquiriesRepository.Add(enquiry);

            if (!String.IsNullOrEmpty(txtEventDetails.Text))
            {
                var note = new EnquiryNote()
                {
                    ID = Guid.NewGuid(),
                    EnquiryID = enquiry.ID,
                    Note = txtEventDetails.Text,
                    Date = DateTime.Now,
                    UserID = defaultSettings.UserID,
                };

                _webEnquiryDataUnit.EnquiryNotesRepository.Add(note);
            }

            var update = new EnquiryUpdate()
            {
                ID = Guid.NewGuid(),
                EnquiryID = enquiry.ID,
                Date = DateTime.Now,
                UserID = defaultSettings.UserID,
                Message = string.Format("Enquiry {0} was created", enquiry.Name)
            };

            _webEnquiryDataUnit.EnquiryUpdatesRepository.Add(update);

            _webEnquiryDataUnit.SaveChanges();

            Alert("Enquiry was successfully sent", Page);

            try
            {
                using (var smtpClient = new SmtpClient(emailSettings.Server))
                {
                    smtpClient.Port = 587;
                    smtpClient.EnableSsl = emailSettings.EnableSSL;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password);

                    using (var message = new MailMessage())
                    {
                        message.From = new MailAddress(defaultSettings.FromAddress);
                        message.IsBodyHtml = true;
                        message.Subject = "Web Enquiry";
                        message.SubjectEncoding = Encoding.UTF8;
                        message.Body = "Enquiry " + enquiry.Name + " has been received.";
                        message.BodyEncoding = Encoding.UTF8;
                        message.To.Add(user.EmailAddress);

                        smtpClient.Send(message);

                    }
                }
            }
            catch (SmtpException)
            {
                Alert("Couldn't send email", Page);
            }
           
            ClearAll();
        }

        private void ClearAll()
        {
            EmptyTextBoxes(form1);
            EventTypesDropDown.ClearSelection();
        }

        public static void Alert(string message, Page page)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(),
           "title",
           "alert('" + message + "');",
           true);
        }

        public static void EmptyTextBoxes(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c.GetType() == typeof(TextBox))
                {
                    ((TextBox)(c)).Text = string.Empty;
                }
            }
        }

        protected void CheckBox1_OnCheckedChanged(object sender, EventArgs e)
        {
            if (DateCheckBox.Checked)
                txtDate.Text = String.Empty;

        }

        protected void txtDate_OnTextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtDate.Text))
                DateCheckBox.Checked = false;
        }
    }

}