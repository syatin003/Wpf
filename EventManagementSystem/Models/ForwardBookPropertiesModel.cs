using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventManagementSystem.Core.ViewModels;

namespace EventManagementSystem.Models
{
    class ForwardBookPropertiesModel : ViewModelBase
    {
        private double _provisional;
        private double _confiremed;
        private double _invoiced;
        private double _cancelled;
        private double _enquiry;
        private double _monthTotal;

        public double Provisional
        {
            get { return _provisional; }
            set
            {
                if (_provisional == value) return;
                _provisional = value;
                RaisePropertyChanged(() => Provisional);
            }
        }

        public double Confirmed
        {
            get { return _confiremed; }
            set
            {
                if (_confiremed == value) return;
                _confiremed = value;
                RaisePropertyChanged(() => Confirmed);
            }
        }

        public double Invoiced
        {
            get { return _invoiced; }
            set
            {
                if (_invoiced == value) return;
                _invoiced = value;
                RaisePropertyChanged(() => Invoiced);
            }
        }

        public double Cancelled
        {
            get { return _cancelled; }
            set
            {
                if (_cancelled == value) return;
                _cancelled = value;
                RaisePropertyChanged(() => Cancelled);
            }
        }

        public double Enquiry
        {
            get { return _enquiry; }
            set
            {
                if (_enquiry == value) return;
                _enquiry = value;
                RaisePropertyChanged(() => Enquiry);
            }
        }

        public double MonthTotal
        {
            get { return _monthTotal; }
            set
            {
                if (_monthTotal == value) return;
                _monthTotal = value;
                RaisePropertyChanged(() => MonthTotal);
            }
        }
    }
}
