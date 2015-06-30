using System;
using System.Windows.Controls;
using EventManagementSystem.Views.Reports.ContentViews;
using Telerik.Windows.Controls;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;

namespace EventManagementSystem.ViewModels.Reports
{
    public class ReportsViewModel : ViewModelBase
    {
        #region Fields

        private object _selectedTreeViewObject;
        private ContentControl _description;
        private ContentControl _options;

        #endregion

        #region Properties

        public ContentControl Description
        {
            get { return _description; }
            set
            {
                if (_description == value) return;
                _description = value;
                RaisePropertyChanged(() => Description);
            }
        }

        public ContentControl Options
        {
            get { return _options; }
            set
            {
                if (_options == value) return;
                _options = value;
                RaisePropertyChanged(() => Options);
            }
        }

        public object SelectedTreeViewObject
        {
            get { return _selectedTreeViewObject; }
            set
            {
                if (_selectedTreeViewObject == value) return;
                _selectedTreeViewObject = value;
                RaisePropertyChanged(() => SelectedTreeViewObject);
                var selectedItem = (RadTreeViewItem)value;
                if (String.Equals(selectedItem.Header.ToString(), "Transactions"))
                {
                    Description = new TransactionsView();
                    Options = null;
                }
                else if (String.Equals(selectedItem.Header.ToString(), "Products"))
                {
                    Description = new ProductsView();
                    Options = null;
                }
                else if (String.Equals(selectedItem.Header.ToString(), "Forward Book"))
                {
                    Description = new ForwardBookDescriptionView();
                    Options = new ForwardBookOptionsView();
                }
                else if (String.Equals(selectedItem.Header.ToString(), "Deposits Received"))
                {
                    Description = new DepositsReceivedDescriptionView();
                    Options = new DepositsReceivedOptionsView();
                }
                else if (String.Equals(selectedItem.Header.ToString(), "Aged Balance"))
                {
                    Description = new AgedBalanceDescriptionView();
                    Options = null;
                }
                else if (String.Equals(selectedItem.Header.ToString(), "Customers"))
                {
                    Description = new CustomersDescriptionView();
                    Options = new CustomersOptionsView();
                }
                else if (String.Equals(selectedItem.Header.ToString(), "Activity"))
                {
                    Description = new ActivityView();
                    Options = null;
                }
                else if (String.Equals(selectedItem.Header.ToString(), "Enquiry Status"))
                {
                    Description = new EnquiryStatusView();
                    Options = null;
                }
                else if (String.Equals(selectedItem.Header.ToString(), "Enquiry Summary"))
                {
                    Description = new EnquirySummary2View();
                    Options = null;
                }
                else if (String.Equals(selectedItem.Header.ToString(), "Payments"))
                {
                    Description = new PaymentReceivedDescriptionView();
                    Options = new PaymentReceivedOptionsView();
                }
                else if (String.Equals(selectedItem.Header.ToString(), "Forward Synopsis"))
                {
                    Description = new ForwardSynopsisDescriptionView();
                    Options = new ForwardSynopsisOptionsView();
                }
                else if (String.Equals(selectedItem.Header.ToString(), "Income"))
                {
                    Description = new RoundAgeAnalysisDescription();
                    Options = new RoundAgeAnalysisOptionsView();
                }
                else if (String.Equals(selectedItem.Header.ToString(), "Calendar"))
                {
                    Description = new CalendarDescriptionView();
                    Options = new CalendarOptionsView();
                }
                else if (String.Equals(selectedItem.Header.ToString(), "Members Count"))
                {
                    Description = new MembersCatCountDescView();
                    Options = null;
                }
                else if (String.Equals(selectedItem.Header.ToString(), "Joiners Leavers"))
                {
                    Description = new JoinersLeaversDescriptionView();
                    Options = new JoinersLeaversOptionsView();
                }
                else if (String.Equals(selectedItem.Header.ToString(), "Joiners Activity"))
                {
                    Description = new JoinersActivityDescriptionView();
                    Options = new JoinersActivityOptionsView();
                }
                else if (String.Equals(selectedItem.Header.ToString(), "Leavers"))
                {
                    Description = new LeaversDescriptionView();
                    Options = new LeaversOptionsView();
                }
                else if (String.Equals(selectedItem.Header.ToString(), "Budget Forecast"))
                {
                    Description = null;
                    Options = null;
                }
                else if (String.Equals(selectedItem.Header.ToString(), "Forward Catering"))
                {
                    Description = null;
                    Options = null;
                }
            }
        }

        #endregion
    }
}