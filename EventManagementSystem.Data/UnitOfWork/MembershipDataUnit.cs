using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories;
using EventManagementSystem.Data.Repositories.Interfaces;
using EventManagementSystem.Data.UnitOfWork.Base;
using EventManagementSystem.Data.UnitOfWork.Interfaces;

namespace EventManagementSystem.Data.UnitOfWork
{
    public class MembershipDataUnit : EntitiesUnitOfWork, IMembershipDataUnit
    {
        public MembershipDataUnit()
            : base(new EmsEntities())
        {
            RegisterRepository<Member, MembersRepository>();
            RegisterRepository<Contact, ContactsRepository>();
            RegisterRepository<ContactTitle, ContactTitlesRepository>();
            RegisterRepository<Corresponcence, CorresponcencesRepository>();
            RegisterRepository<CCContactsCorrespondence, CCContactsCorrespondenceRepository>();
            RegisterRepository<CorresponcenceType, CorresponcenceTypesRepository>();
            RegisterRepository<MembershipCategory, MembershipCategoriesRepository>();
            RegisterRepository<MemberNote, MemberNotesRepository>();
            RegisterRepository<MembershipUpdate, MembershipUpdatesRepository>();
            RegisterRepository<MailTemplate, MailTemplatesRepository>();
            RegisterRepository<EmailHeader, EmailHeadersRepository>();
            RegisterRepository<SystemSetting, SystemSettingsRepository>();
        }

        public IMembersRepository MembersRepository
        {
            get { return (IMembersRepository)GetRepository<Member>(); }
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

        public IContactsRepository ContactsRepository
        {
            get { return (IContactsRepository)GetRepository<Contact>(); }
        }

        public ICCContactsCorrespondenceRepository CCContactsCorrespondenceRepository
        {
            get { return (ICCContactsCorrespondenceRepository)GetRepository<CCContactsCorrespondence>(); }
        }

        public IMembershipCategoriesRepository MembershipCategoriesRepository
        {
            get { return (IMembershipCategoriesRepository)GetRepository<MembershipCategory>(); }
        }
        public IMemberNotesRepository MemberNotesRepository
        {
            get { return (IMemberNotesRepository)GetRepository<MemberNote>(); }
        }
        public IMembershipUpdatesRepository MembershipUpdatesRepository
        {
            get { return (IMembershipUpdatesRepository)GetRepository<MembershipUpdate>(); }
        }

        public IMailTemplatesRepository MailTemplatesRepository
        {
            get { return (IMailTemplatesRepository)GetRepository<MailTemplate>(); }
        }

        public IEmailHeadersRepository EmailHeadersRepository
        {
            get { return (IEmailHeadersRepository)GetRepository<EmailHeader>(); }
        }

        public ISystemSettingsRepository SystemSettingsRepository
        {
            get { return (ISystemSettingsRepository)GetRepository<SystemSetting>(); }
        }
    }
}
