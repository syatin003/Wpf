using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories;
using EventManagementSystem.Data.Repositories.Interfaces;
using EventManagementSystem.Data.UnitOfWork.Base;
using EventManagementSystem.Data.UnitOfWork.Interfaces;

namespace EventManagementSystem.Data.UnitOfWork
{
    public class WebEnquiryDataUnit : EntitiesUnitOfWork, IWebEnquiryDataUnit
    {
        public WebEnquiryDataUnit() 
            : base(new EmsEntities())
        {
            RegisterRepository<EventType, EventTypesRepository>();
            RegisterRepository<DefaultSettingsForEnquiry, DefaultSettingsForEnquiriesRepository>();
            RegisterRepository<User, UsersRepository>();
            RegisterRepository<EnquiryReceiveMethod, EnquiryReceiveMethodsRepository>();
            RegisterRepository<EventStatus, EventStatusesRepository>();
            RegisterRepository<CorresponcenceType, CorresponcenceTypesRepository>();
            RegisterRepository<Contact, ContactsRepository>();
            RegisterRepository<Enquiry, EnquiriesRepository>();
            //RegisterRepository<EnquiryDetail, EnquiryDetailsRepository>();
            RegisterRepository<EnquiryNote, EnquiryNotesRepository>();
            RegisterRepository<EnquiryUpdate, EnquiryUpdatesRepository>();
            RegisterRepository<Corresponcence, CorresponcencesRepository>();
            RegisterRepository<EmailSetting, EmailSettingsRepository>();
        }

        public ICorresponcencesRepository CorresponcencesRepository
        {
            get { return (ICorresponcencesRepository)GetRepository<Corresponcence>(); }
        }

        public IEnquiryUpdatesRepository EnquiryUpdatesRepository
        {
            get { return (IEnquiryUpdatesRepository)GetRepository<EnquiryUpdate>(); }
        }

        public IEnquiryNotesRepository EnquiryNotesRepository
        {
            get { return (IEnquiryNotesRepository)GetRepository<EnquiryNote>(); }
        }

        public IEnquiriesRepository EnquiriesRepository
        {
            get { return (IEnquiriesRepository)GetRepository<Enquiry>(); }
        }

        public IContactsRepository ContactsRepository
        {
            get { return (IContactsRepository)GetRepository<Contact>(); }
        }

        public ICorresponcenceTypesRepository CorresponcenceTypesRepository
        {
            get { return (ICorresponcenceTypesRepository)GetRepository<CorresponcenceType>(); }
        }

        public IEventStatusesRepository EventStatusesRepository
        {
            get { return (IEventStatusesRepository)GetRepository<EventStatus>(); }
        }

        public IEnquiryReceiveMethodsRepository EnquiryReceiveMethodsRepository
        {
            get { return (IEnquiryReceiveMethodsRepository)GetRepository<EnquiryReceiveMethod>(); }
        }

        public IUsersRepository UsersRepository
        {
            get { return (IUsersRepository)GetRepository<User>(); }
        }

        public IDefaultSettingsForEnquiriesRepository DefaultSettingsForEnquiriesRepository
        {
            get { return (IDefaultSettingsForEnquiriesRepository)GetRepository<DefaultSettingsForEnquiry>(); }
        }

        public IEventTypesRepository EventTypesRepository 
        { 
            get { return (IEventTypesRepository) GetRepository<EventType>();}
        }

        public IEmailSettingsRepository EmailSettingsRepository
        {
            get { return (IEmailSettingsRepository) GetRepository<EmailSetting>(); }
        }
    }
}
