using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories;
using EventManagementSystem.Data.Repositories.Interfaces;
using EventManagementSystem.Data.UnitOfWork.Base;
using EventManagementSystem.Data.UnitOfWork.Interfaces;

namespace EventManagementSystem.Data.UnitOfWork
{
    public class CrmDataUnit : EntitiesUnitOfWork, ICrmDataUnit
    {
        public CrmDataUnit()
            : base(new EmsEntities())
        {
            RegisterRepository<CampaignType, CampaignTypesRepository>();
            RegisterRepository<Campaign, CampaignsRepository>();
            RegisterRepository<FollowUpStatus, FollowUpStatusesRepository>();
            RegisterRepository<Enquiry, EnquiriesRepository>();
            RegisterRepository<FollowUp, FollowUpsRepository>();
            RegisterRepository<Activity, ActivitiesRepository>();
            RegisterRepository<EnquiryUpdate, EnquiryUpdatesRepository>();
            RegisterRepository<EventStatus, EventStatusesRepository>();
            RegisterRepository<EventType, EventTypesRepository>();
            RegisterRepository<EventNoteType, EventNoteTypesRepository>();
            RegisterRepository<EventNote, EventNotesRepository>();
            RegisterRepository<Corresponcence, CorresponcencesRepository>();
            RegisterRepository<User, UsersRepository>();
            RegisterRepository<EnquiryStatus, EnquiryStatusesRepository>();
            RegisterRepository<EnquiryReceiveMethod, EnquiryReceiveMethodsRepository>();
            RegisterRepository<EnquiryNote, EnquiryNotesRepository>();
            RegisterRepository<ActivityType, ActivityTypesRepository>();
            RegisterRepository<CorresponcenceType, CorresponcenceTypesRepository>();
            RegisterRepository<Contact, ContactsRepository>();
            RegisterRepository<Event, EventsRepository>();
            RegisterRepository<EventUpdate, EventUpdatesRepository>();
            RegisterRepository<MailTemplate, MailTemplatesRepository>();
            RegisterRepository<Document, DocumentsRepository>();
            RegisterRepository<EventContact, EventContactsRepository>();
            RegisterRepository<CCContactsCorrespondence, CCContactsCorrespondenceRepository>();
            RegisterRepository<CorrespondenceDocument, CorrespondenceDocumentsRepository>();
            RegisterRepository<EmailHeader, EmailHeadersRepository>();
        }

        public IEventContactsRepository EventContactsRepository
        {
            get { return (IEventContactsRepository)GetRepository<EventContact>(); }
        }

        public IEventUpdatesRepository EventUpdatesRepository
        {
            get { return (IEventUpdatesRepository)GetRepository<EventUpdate>(); }
        }

        public IEventsRepository EventsRepository
        {
            get { return (IEventsRepository)GetRepository<Event>(); }
        }

        public IContactsRepository ContactsRepository
        {
            get { return (IContactsRepository)GetRepository<Contact>(); }
        }

        public ICorresponcenceTypesRepository CorresponcenceTypesRepository
        {
            get { return (ICorresponcenceTypesRepository)GetRepository<CorresponcenceType>(); }
        }

        public IActivityTypesRepository ActivityTypesRepository
        {
            get { return (IActivityTypesRepository)GetRepository<ActivityType>(); }
        }

        public IEnquiryNotesRepository EnquiryNotesRepository
        {
            get { return (IEnquiryNotesRepository)GetRepository<EnquiryNote>(); }
        }

        public IEnquiryReceiveMethodsRepository EnquiryReceiveMethodsRepository
        {
            get { return (IEnquiryReceiveMethodsRepository)GetRepository<EnquiryReceiveMethod>(); }
        }

        public IEnquiryStatusesRepository EnquiryStatusesRepository
        {
            get { return (IEnquiryStatusesRepository)GetRepository<EnquiryStatus>(); }
        }

        public IUsersRepository UsersRepository
        {
            get { return (IUsersRepository)GetRepository<User>(); }
        }

        public ICorresponcencesRepository CorresponcencesRepository
        {
            get { return (ICorresponcencesRepository)GetRepository<Corresponcence>(); }
        }

        public IEventNotesRepository EventNotesRepository
        {
            get { return (IEventNotesRepository)GetRepository<EventNote>(); }
        }

        public IEventNoteTypesRepository EventNoteTypesRepository
        {
            get { return (IEventNoteTypesRepository)GetRepository<EventNoteType>(); }
        }

        public IEventTypesRepository EventTypesRepository
        {
            get { return (IEventTypesRepository)GetRepository<EventType>(); }
        }

        public IEventStatusesRepository EventStatusesRepository
        {
            get { return (IEventStatusesRepository)GetRepository<EventStatus>(); }
        }

        public IEnquiryUpdatesRepository EnquiryUpdatesRepository
        {
            get { return (IEnquiryUpdatesRepository)GetRepository<EnquiryUpdate>(); }
        }

        public IActivitiesRepository ActivitiesRepository
        {
            get { return (IActivitiesRepository)GetRepository<Activity>(); }
        }

        public IFollowUpsRepository FollowUpsRepository
        {
            get { return (IFollowUpsRepository)GetRepository<FollowUp>(); }
        }

        public IEnquiriesRepository EnquiriesRepository
        {
            get { return (IEnquiriesRepository)GetRepository<Enquiry>(); }
        }

        public IFollowUpStatusesRepository FollowUpStatusesRepository
        {
            get { return (IFollowUpStatusesRepository)GetRepository<FollowUpStatus>(); }
        }

        public ICampaignsRepository CampaignsRepository
        {
            get { return (ICampaignsRepository)GetRepository<Campaign>(); }
        }

        public ICampaignTypesRepository CampaignTypesRepository
        {
            get { return (ICampaignTypesRepository)GetRepository<CampaignType>(); }
        }

        public IMailTemplatesRepository MailTemplatesRepository
        {
            get { return (IMailTemplatesRepository)GetRepository<MailTemplate>(); }
        }

        public IDocumentsRepository DocumentsRepository
        {
            get { return (IDocumentsRepository)GetRepository<Document>(); }
        }

        public ICCContactsCorrespondenceRepository CCContactsCorrespondenceRepository
        {
            get { return (ICCContactsCorrespondenceRepository)GetRepository<CCContactsCorrespondence>(); }
        }

        public ICorrespondenceDocumentsRepository CorrespondenceDocumentsRepository
        {
            get { return (ICorrespondenceDocumentsRepository)GetRepository<CorrespondenceDocument>(); }
        }

        public IEmailHeadersRepository EmailHeadersRepository
        {
            get { return (IEmailHeadersRepository)GetRepository<EmailHeader>(); }
        }
    }
}
