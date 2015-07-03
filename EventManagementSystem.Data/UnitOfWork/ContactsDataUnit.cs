using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories;
using EventManagementSystem.Data.Repositories.Interfaces;
using EventManagementSystem.Data.UnitOfWork.Base;
using EventManagementSystem.Data.UnitOfWork.Interfaces;

namespace EventManagementSystem.Data.UnitOfWork
{
    public class ContactsDataUnit : EntitiesUnitOfWork, IContactsDataUnit
    {
        public ContactsDataUnit()
            : base(new EmsEntities())
        {
            RegisterRepository<Contact, ContactsRepository>();
            RegisterRepository<Event, EventsRepository>();
            RegisterRepository<Enquiry, EnquiriesRepository>();
            RegisterRepository<ContactTitle, ContactTitlesRepository>();
            RegisterRepository<Corresponcence, CorresponcencesRepository>();
            RegisterRepository<CorresponcenceType, CorresponcenceTypesRepository>();
            RegisterRepository<EventType, EventTypesRepository>();
            RegisterRepository<EventUpdate, EventUpdatesRepository>();
            RegisterRepository<Invoice, InvoicesRepository>();
            RegisterRepository<EventPayment, EventPaymentsRepository>();
            RegisterRepository<CCContactsCorrespondence, CCContactsCorrespondenceRepository>();

            RegisterRepository<MailTemplate, MailTemplatesRepository>();
            RegisterRepository<EmailHeader, EmailHeadersRepository>();
            RegisterRepository<ContactUpdate, ContactUpdatesRepository>();

         }

        public IEventPaymentsRepository EventPaymentsRepository
        {
            get { return (IEventPaymentsRepository)GetRepository<EventPayment>(); }
        }

        public IInvoicesRepository InvoicesRepository
        {
            get { return (IInvoicesRepository)GetRepository<Invoice>(); }
        }

        public IEventUpdatesRepository EventUpdatesRepository
        {
            get { return (IEventUpdatesRepository)GetRepository<EventUpdate>(); }
        }

        public IEventTypesRepository EventTypesRepository
        {
            get { return (IEventTypesRepository)GetRepository<EventType>(); }
        }

        public ICorresponcencesRepository CorresponcencesRepository
        {
            get { return (ICorresponcencesRepository)GetRepository<Corresponcence>(); }
        }

        public ICorresponcenceTypesRepository CorresponcenceTypesRepository
        {
            get { return (ICorresponcenceTypesRepository)GetRepository<CorresponcenceType>(); }
        }

        public IContactTitlesRepository ContactTitlesRepository
        {
            get { return (IContactTitlesRepository)GetRepository<ContactTitle>(); }
        }

        public IEnquiriesRepository EnquiriesRepository
        {
            get { return (IEnquiriesRepository)GetRepository<Enquiry>(); }
        }

        public IEventsRepository EventsRepository
        {
            get { return (IEventsRepository)GetRepository<Event>(); }
        }

        public IContactsRepository ContactsRepository
        {
            get { return (IContactsRepository)GetRepository<Contact>(); }
        }

        public ICCContactsCorrespondenceRepository CCContactsCorrespondenceRepository
        {
            get { return (ICCContactsCorrespondenceRepository) GetRepository<CCContactsCorrespondence>(); }
        }


        public IMailTemplatesRepository MailTemplatesRepository
        {
            get { return (IMailTemplatesRepository)GetRepository<MailTemplate>(); }
        }

        public IEmailHeadersRepository EmailHeadersRepository
        {
            get { return (IEmailHeadersRepository)GetRepository<EmailHeader>(); }
        }


        public IContactUpdatesRepository ContactUpdatesRepository
        {
            get { return (IContactUpdatesRepository)GetRepository<ContactUpdate>(); }
        }
    }
}
