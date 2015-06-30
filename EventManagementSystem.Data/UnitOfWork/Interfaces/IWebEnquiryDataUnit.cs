using EventManagementSystem.Data.Repositories.Interfaces;
using EventManagementSystem.Data.UnitOfWork.Base;

namespace EventManagementSystem.Data.UnitOfWork.Interfaces
{
    public interface IWebEnquiryDataUnit : IDataUnitOfWork
    {
        IEventTypesRepository EventTypesRepository { get; }
        IDefaultSettingsForEnquiriesRepository DefaultSettingsForEnquiriesRepository { get; }
        IUsersRepository UsersRepository { get; }
        IEnquiryReceiveMethodsRepository EnquiryReceiveMethodsRepository { get; }
        IEventStatusesRepository EventStatusesRepository { get; }
        ICorresponcenceTypesRepository CorresponcenceTypesRepository { get; }
        IContactsRepository ContactsRepository { get; }
        IEnquiriesRepository EnquiriesRepository { get; }
        IEnquiryNotesRepository EnquiryNotesRepository { get; }
        IEnquiryUpdatesRepository EnquiryUpdatesRepository { get; }
        ICorresponcencesRepository CorresponcencesRepository { get; }
        IEmailSettingsRepository EmailSettingsRepository { get; }
    }
}
