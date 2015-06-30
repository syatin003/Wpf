using System;
using System.Collections.ObjectModel;
using EventManagementSystem.Core.ViewModels;

namespace EventManagementSystem.Models
{
    [Serializable]
    public class EventItemModel : ModelBase
    {
        #region Fields

        private ObservableCollection<EventBookedProductModel> _products;
        private string _note;
        private string _room;

        #endregion

        #region Properties

        public object Instance { get; set; }
        public DateTime? Time { get; set; }
        public string Title { get; set; }
        public double TotalPrice { get; set; }
        public bool ShowInInvoice { get; set; }
        public bool IncludeInCorrespondence { get; set; }


        public string Room
        {
            get { return _room; }
            set
            {
                if (_room == value) return;
                _room = value;
                RaisePropertyChanged(() => Room);
            }
        }

        public string Note
        {
            get { return _note; }
            set
            {
                SetNote(value);
                RaisePropertyChanged(() => Note);
            }
        }

        public ObservableCollection<EventBookedProductModel> Products
        {
            get { return _products; }
            set
            {
                if (_products == value) return;
                _products = value;
                RaisePropertyChanged(() => Products);
            }
        }

        #endregion

        #region Methods

        private void SetNote(string note)
        {
            if (!string.IsNullOrWhiteSpace(note))
                _note = (note.Length <= 120) ? note : string.Format("{0} ...", note.Substring(0, (note.Length <= 120) ? note.Length : 120));
        }

        #endregion
    }
}
