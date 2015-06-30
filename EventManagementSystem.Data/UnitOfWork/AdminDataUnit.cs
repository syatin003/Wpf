using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories;
using EventManagementSystem.Data.Repositories.Interfaces;
using EventManagementSystem.Data.UnitOfWork.Base;
using EventManagementSystem.Data.UnitOfWork.Interfaces;

namespace EventManagementSystem.Data.UnitOfWork
{
    public class AdminDataUnit : EntitiesUnitOfWork, IAdminDataUnit
    {
        public AdminDataUnit()
            : base(new EmsEntities())
        {
            RegisterRepository<Room, RoomsRepository>();
            RegisterRepository<Facility, FacilitiesRepository>();
            RegisterRepository<RoomFacility, RoomFacilitiesRepository>();
            RegisterRepository<Golf, GolfsRepository>();
            RegisterRepository<GolfFollowResource, GolfFollowResourceRepository>();

            RegisterRepository<User, UsersRepository>();
            RegisterRepository<UserGroup, UserGroupsRepository>();
            RegisterRepository<UserJobType, UserJobTypesRepository>();
            RegisterRepository<UserDepartment, UserDepartmentsRepository>();
            RegisterRepository<Permission, PermissionsRepository>();
            RegisterRepository<UserPermission, UserPermissionsRepository>();
            RegisterRepository<UserGroupPermission, UserGroupPermissionsRepository>();
            RegisterRepository<PermissionGroup, PermissionGroupsRepository>();

            RegisterRepository<EventOption, EventOptionsRepository>();
            RegisterRepository<EventTypeOption, EventTypeOptionsRepository>();
            RegisterRepository<EventType, EventTypesRepository>();
            RegisterRepository<EventStatus, EventStatusesRepository>();
            RegisterRepository<EventStatusOption, EventStatusOptionsRepository>();

            RegisterRepository<Product, ProductsRepository>();
            RegisterRepository<ProductType, ProductTypesRepository>();
            RegisterRepository<ProductEventType, ProductEventTypesRepository>();
            RegisterRepository<ProductDepartment, ProductDepartmentsRepository>();
            RegisterRepository<ProductGroup, ProductGroupsRepository>();
            RegisterRepository<ProductVATRate, ProductVatRatesRepository>();
            RegisterRepository<ProductOption, ProductOptionsRepository>();

            RegisterRepository<MailTemplate, MailTemplatesRepository>();
            RegisterRepository<MailTemplateCategory, MailTemplateCategoriesRepository>();
            RegisterRepository<MailTemplateType, MailTemplateTypesRepository>();

            RegisterRepository<FollowUpStatus, FollowUpStatusesRepository>();
            RegisterRepository<EnquiryStatus, EnquiryStatusesRepository>();
            RegisterRepository<EnquiryReceiveMethod, EnquiryReceiveMethodsRepository>();

            RegisterRepository<DefaultSettingsForEnquiry, DefaultSettingsForEnquiriesRepository>();
            RegisterRepository<Document, DocumentsRepository>();
            RegisterRepository<Event, EventsRepository>();

            RegisterRepository<TillDivision, TillDivisionsRepository>();
            RegisterRepository<Till, TillsRepository>();
            RegisterRepository<SystemSetting, SystemSettingsRepository>();
            RegisterRepository<EventTypeTODO, EventTypeTODOsRepository>();
            RegisterRepository<MembershipGroupStyle, MembershipGroupStylesRepository>();
            RegisterRepository<MembershipGroupAge, MembershipGroupAgesRepository>();
            RegisterRepository<MembershipGroup, MembershipGroupsRepository>();
            RegisterRepository<MembershipCategory, MembershipCategoriesRepository>();
            RegisterRepository<MembershipCategoryGroupDefault, MembershipCategoryGroupDefaultsRepository>();
            RegisterRepository<MembershipFullPaymentComponent, MembershipFullPaymentCostsRepository>();
            RegisterRepository<MembershipMonthlyPaymentOngoingCost, MembershipMonthlyPaymentOngoingCostsRepository>();
            RegisterRepository<MembershipMonthlyPaymentUpFrontCost, MembershipMonthlyPaymentUpFrontCostsRepository>();
            RegisterRepository<MembershipToken, MembershipTokensRepository>();
            RegisterRepository<MembershipLinkType, MembershipLinkTypesRepository>();
            RegisterRepository<MembershipOptionBox, MembershipOptionBoxesRepository>();
            RegisterRepository<MembershipOptionBoxReason, MembershipOptionBoxReasonsRepository>();
            RegisterRepository<MembershipGroupEPOS, MembershipGroupEPOSRepository>();
            RegisterRepository<MembershipCategoryGroupDefaultEPOS, MembershipCategoryGroupDefaultEPOSRepository>();
            RegisterRepository<EmailHeader, EmailHeadersRepository>();

        }

        public IEventsRepository EventsRepository
        {
            get { return (IEventsRepository)GetRepository<Event>(); }
        }

        public IRoomFacilitiesRepository RoomFacilitiesRepository
        {
            get { return (IRoomFacilitiesRepository)GetRepository<RoomFacility>(); }
        }

        public IProductVatRatesRepository ProductVatRatesRepository
        {
            get { return (IProductVatRatesRepository)GetRepository<ProductVATRate>(); }
        }

        public IProductOptionsRepository ProductOptionsRepository
        {
            get { return (IProductOptionsRepository)GetRepository<ProductOption>(); }
        }

        public IFacilitiesRepository FacilitiesRepository
        {
            get { return (IFacilitiesRepository)GetRepository<Facility>(); }
        }

        public IUserGroupsRepository UserGroupsRepository
        {
            get { return (IUserGroupsRepository)GetRepository<UserGroup>(); }
        }

        public IUserJobTypesRepository UserJobTypesRepository
        {
            get { return (IUserJobTypesRepository)GetRepository<UserJobType>(); }
        }

        public IUserDepartmentsRepository UserDepartmentsRepository
        {
            get { return (IUserDepartmentsRepository)GetRepository<UserDepartment>(); }
        }

        public IPermissionsRepository PermissionsRepository
        {
            get { return (IPermissionsRepository)GetRepository<Permission>(); }
        }

        public IUserPermissionsRepository UserPermissionsRepository
        {
            get { return (IUserPermissionsRepository)GetRepository<UserPermission>(); }
        }

        public IUserGroupPermissionsRepository UserGroupPermissionsRepository
        {
            get { return (IUserGroupPermissionsRepository)GetRepository<UserGroupPermission>(); }
        }

        public IPermissionGroupsRepository PermissionGroupsRepository
        {
            get { return (IPermissionGroupsRepository)GetRepository<PermissionGroup>(); }
        }

        public IEventOptionsRepository EventOptionsRepository
        {
            get { return (IEventOptionsRepository)GetRepository<EventOption>(); }
        }

        public IEventTypeOptionsRepository EventTypeOptionsRepository
        {
            get { return (IEventTypeOptionsRepository)GetRepository<EventTypeOption>(); }
        }

        public IEventStatusOptionsRepository EventStatusOptionsRepository
        {
            get { return (IEventStatusOptionsRepository)GetRepository<EventStatusOption>(); }
        }

        public IRoomsRepository RoomsRepository
        {
            get { return (IRoomsRepository)GetRepository<Room>(); }
        }

        public IProductsRepository ProductsRepository
        {
            get { return (IProductsRepository)GetRepository<Product>(); }
        }

        public IGolfHolesRepository GolfHolesRepository
        {
            get { return (IGolfHolesRepository)GetRepository<GolfHole>(); }
        }

        public IGolfsRepository GolfsRepository
        {
            get { return (IGolfsRepository)GetRepository<Golf>(); }
        }

        public IGolfFollowResourcesRepository GolfFollowResourceRepository
        {
            get { return (IGolfFollowResourcesRepository)GetRepository<GolfFollowResource>(); }
        }

        public IUsersRepository UsersRepository
        {
            get { return (IUsersRepository)GetRepository<User>(); }
        }

        public IMailTemplatesRepository MailTemplatesRepository
        {
            get { return (IMailTemplatesRepository)GetRepository<MailTemplate>(); }
        }

        public IMailTemplateCategoriesRepository MailTemplateCategoriesRepository
        {
            get { return (IMailTemplateCategoriesRepository)GetRepository<MailTemplateCategory>(); }
        }

        public IMailTemplateTypesRepository MailTemplateTypesRepository
        {
            get { return (IMailTemplateTypesRepository)GetRepository<MailTemplateType>(); }
        }

        public IProductTypesRepository ProductTypesRepository
        {
            get { return (IProductTypesRepository)GetRepository<ProductType>(); }
        }

        public IProductGroupsRepository ProductGroupsRepository
        {
            get { return (IProductGroupsRepository)GetRepository<ProductGroup>(); }
        }

        public IProductDepartmentsRepository ProductDepartmentsRepository
        {
            get { return (IProductDepartmentsRepository)GetRepository<ProductDepartment>(); }
        }

        public IProductEventTypesRepository ProductEventTypesRepository
        {
            get { return (IProductEventTypesRepository)GetRepository<ProductEventType>(); }
        }

        public IEventTypesRepository EventTypesRepository
        {
            get { return (IEventTypesRepository)GetRepository<EventType>(); }
        }

        public IEventStatusesRepository EventStatusesRepository
        {
            get { return (IEventStatusesRepository)GetRepository<EventStatus>(); }
        }

        public IFollowUpStatusesRepository FollowUpStatusesRepository
        {
            get { return (IFollowUpStatusesRepository)GetRepository<FollowUpStatus>(); }
        }

        public IEnquiryStatusesRepository EnquiryStatusesRepository
        {
            get { return (IEnquiryStatusesRepository)GetRepository<EnquiryStatus>(); }
        }

        public IEnquiryReceiveMethodsRepository EnquiryReceiveMethodsRepository
        {
            get { return (IEnquiryReceiveMethodsRepository)GetRepository<EnquiryReceiveMethod>(); }
        }

        public IDefaultSettingsForEnquiriesRepository DefaultSettingsForEnquiriesRepository
        {
            get { return (IDefaultSettingsForEnquiriesRepository)GetRepository<DefaultSettingsForEnquiry>(); }
        }

        public IDocumentsRepository DocumentsRepository
        {
            get { return (IDocumentsRepository)GetRepository<Document>(); }
        }

        public ITillDivisionsRepository TillDivisionsRepository
        {
            get { return (ITillDivisionsRepository)GetRepository<TillDivision>(); }
        }
        public ITillsRepository TillsRepository
        {
            get { return (ITillsRepository)GetRepository<Till>(); }
        }
        public ISystemSettingsRepository SystemSettingsRepository
        {
            get { return (ISystemSettingsRepository)GetRepository<SystemSetting>(); }
        }
        public IEventTypeTODOsRepository EventTypeTODOsRepository
        {
            get { return (IEventTypeTODOsRepository)GetRepository<EventTypeTODO>(); }
        }
        public IMembershipGroupStylesRepository MembershipGroupStylesRepository
        {
            get { return (IMembershipGroupStylesRepository)GetRepository<MembershipGroupStyle>(); }
        }
        public IMembershipGroupAgesRepository MembershipGroupAgesRepository
        {
            get { return (IMembershipGroupAgesRepository)GetRepository<MembershipGroupAge>(); }
        }
        public IMembershipGroupsRepository MembershipGroupsRepository
        {
            get { return (IMembershipGroupsRepository)GetRepository<MembershipGroup>(); }
        }
        public IMembershipCategoriesRepository MembershipCategoriesRepository
        {
            get { return (IMembershipCategoriesRepository)GetRepository<MembershipCategory>(); }
        }
        public IMembershipCategoryGroupDefaultsRepository MembershipCategoryGroupDefaultsRepository
        {
            get { return (IMembershipCategoryGroupDefaultsRepository)GetRepository<MembershipCategoryGroupDefault>(); }

        }
        public IMembershipFullPaymentCostsRepository MembershipFullPaymentCostsRepository
        {
            get { return (IMembershipFullPaymentCostsRepository)GetRepository<MembershipFullPaymentComponent>(); }

        }
        public IMembershipMonthlyPaymentOngoingCostsRepository MembershipMonthlyPaymentOngoingCostsRepository
        {
            get { return (IMembershipMonthlyPaymentOngoingCostsRepository)GetRepository<MembershipMonthlyPaymentOngoingCost>(); }

        }

        public IMembershipMonthlyPaymentUpFrontCostsRepository MembershipMonthlyPaymentUpFrontCostsRepository
        {
            get { return (IMembershipMonthlyPaymentUpFrontCostsRepository)GetRepository<MembershipMonthlyPaymentUpFrontCost>(); }

        }

        public IMembershipTokensRepository MembershipTokensRepository
        {
            get { return (IMembershipTokensRepository)GetRepository<MembershipToken>(); }

        }

        public IMembershipLinkTypesRepository MembershipLinkTypesRepository
        {
            get { return (IMembershipLinkTypesRepository)GetRepository<MembershipLinkType>(); }
        }

        public IMembershipOptionBoxesRepository MembershipOptionBoxesRepository
        {
            get { return (IMembershipOptionBoxesRepository)GetRepository<MembershipOptionBox>(); }
        }

        public IMembershipOptionBoxReasonsRepository MembershipOptionBoxReasonsRepository
        {
            get { return (IMembershipOptionBoxReasonsRepository)GetRepository<MembershipOptionBoxReason>(); }
        }

        public IMembershipGroupEPOSRepository MembershipGroupEPOSRepository
        {
            get { return (IMembershipGroupEPOSRepository)GetRepository<MembershipGroupEPOS>(); }
        }

        public IMembershipCategoryGroupDefaultEPOSRepository MembershipCategoryGroupDefaultEPOSRepository
        {
            get { return (IMembershipCategoryGroupDefaultEPOSRepository)GetRepository<MembershipCategoryGroupDefaultEPOS>(); }
        }

        public IEmailHeadersRepository EmailHeadersRepository
        {
            get { return (IEmailHeadersRepository)GetRepository<EmailHeader>(); }
        }
    }
}
