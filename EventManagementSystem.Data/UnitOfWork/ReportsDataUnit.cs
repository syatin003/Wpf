using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.Repositories;
using EventManagementSystem.Data.Repositories.Interfaces;
using EventManagementSystem.Data.UnitOfWork.Base;
using EventManagementSystem.Data.UnitOfWork.Interfaces;

namespace EventManagementSystem.Data.UnitOfWork
{
    public class ReportsDataUnit : EntitiesUnitOfWork, IReportsDataUnit
    {
        public ReportsDataUnit()
            : base(new EmsEntities())
        {
            RegisterRepository<Activity, ActivitiesRepository>();
            RegisterRepository<ActivityType, ActivityTypesRepository>();
            RegisterRepository<EventType, EventTypesRepository>();
            RegisterRepository<Enquiry, EnquiriesRepository>();
            RegisterRepository<Event, EventsRepository>();
            RegisterRepository<EventPayment, EventPaymentsRepository>();
            RegisterRepository<ProductVATRate, ProductVatRatesRepository>();
            RegisterRepository<ProductGroup, ProductGroupsRepository>();
            RegisterRepository<ProductDepartment, ProductDepartmentsRepository>();
            RegisterRepository<EventBookedProduct, EventBookedProductsRepository>();
            RegisterRepository<Till, TillsRepository>();
            RegisterRepository<FinaliseKey, FinaliseKeysRepository>();
            RegisterRepository<TillProduct, TillProductsRepository>();
            RegisterRepository<Clerk, ClerksRepository>();
            RegisterRepository<TillTransaction, TillTransactionsRepository>();
            RegisterRepository<TillTransactionProduct, TillTransactionProductsRepository>();
            RegisterRepository<TillTransactionFinaliseKey, TillTransactionFinaliseKeysRepository>();
            RegisterRepository<Product, ProductsRepository>();
            RegisterRepository<Member, MembersRepository>();
        }

        public ITillTransactionFinaliseKeysRepository TillTransactionFinaliseKeysRepository
        {
            get { return (ITillTransactionFinaliseKeysRepository)GetRepository<TillTransactionFinaliseKey>(); }
        }

        public ITillTransactionProductsRepository TillTransactionProductsRepository
        {
            get { return (ITillTransactionProductsRepository)GetRepository<TillTransactionProduct>(); }
        }

        public ITillTransactionsRepository TillTransactionsRepository
        {
            get { return (ITillTransactionsRepository)GetRepository<TillTransaction>(); }
        }

        public IClerksRepository ClerksRepository
        {
            get { return (IClerksRepository)GetRepository<Clerk>(); }
        }

        public ITillProductsRepository TillProductsRepository
        {
            get { return (ITillProductsRepository)GetRepository<TillProduct>(); }
        }

        public IFinaliseKeysRepository FinaliseKeysRepository
        {
            get { return (IFinaliseKeysRepository)GetRepository<FinaliseKey>(); }
        }

        public ITillsRepository TillsRepository
        {
            get { return (ITillsRepository)GetRepository<Till>(); }
        }

        public IActivitiesRepository ActivitiesRepository
        {
            get { return (IActivitiesRepository)GetRepository<Activity>(); }
        }

        public IActivityTypesRepository ActivityTypesRepository
        {
            get { return (IActivityTypesRepository)GetRepository<ActivityType>(); }
        }

        public IEventTypesRepository EventTypesRepository
        {
            get { return (IEventTypesRepository)GetRepository<EventType>(); }
        }

        public IEnquiriesRepository EnquiriesRepository
        {
            get { return (IEnquiriesRepository)GetRepository<Enquiry>(); }
        }

        public IEventsRepository EventsRepository
        {
            get { return (IEventsRepository)GetRepository<Event>(); }
        }

        public IEventPaymentsRepository EventPaymentsRepository
        {
            get { return (IEventPaymentsRepository)GetRepository<EventPayment>(); }
        }

        public IProductVatRatesRepository ProductVatRatesRepository
        {
            get { return (IProductVatRatesRepository)GetRepository<ProductVATRate>(); }
        }

        public IProductGroupsRepository ProductGroupsRepository
        {
            get { return (IProductGroupsRepository)GetRepository<ProductGroup>(); }
        }

        public IProductDepartmentsRepository ProductDepartmentsRepository
        {
            get { return (IProductDepartmentsRepository)GetRepository<ProductDepartment>(); }
        }

        public IEventBookedProductsRepository EventBookedProductsRepository
        {
            get { return (IEventBookedProductsRepository)GetRepository<EventBookedProduct>(); }
        }


        public IProductsRepository ProductsRepository
        {
            get { return (IProductsRepository)GetRepository<Product>(); }
        }

        public IMembersRepository MembersRepository
        {
            get { return (IMembersRepository)GetRepository<Member>(); }

        }
    }
}
