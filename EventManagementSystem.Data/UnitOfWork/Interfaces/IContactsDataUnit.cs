using EventManagementSystem.Data.Repositories.Interfaces;
using EventManagementSystem.Data.UnitOfWork.Base;

namespace EventManagementSystem.Data.UnitOfWork.Interfaces
{
    public interface IContactsDataUnit : IDataUnitOfWork
    {
        IContactsRepository ContactsRepository { get; }
        IEventsRepository EventsRepository { get; }
        IEnquiriesRepository EnquiriesRepository { get; }
        IContactTitlesRepository ContactTitlesRepository { get; }
        ICorresponcenceTypesRepository CorresponcenceTypesRepository { get; }
        ICorresponcencesRepository CorresponcencesRepository { get; }
        ICCContactsCorrespondenceRepository  CCContactsCorrespondenceRepository { get; }
        IEventTypesRepository EventTypesRepository { get; }
        IEventUpdatesRepository EventUpdatesRepository { get; }
        IInvoicesRepository InvoicesRepository { get; }
        IEventPaymentsRepository EventPaymentsRepository { get; }

        IMailTemplatesRepository MailTemplatesRepository { get; }
        IEmailHeadersRepository EmailHeadersRepository { get; }
        IContactUpdatesRepository ContactUpdatesRepository { get; }

    }
}
