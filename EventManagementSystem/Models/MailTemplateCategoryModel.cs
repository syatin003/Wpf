using System.Collections.ObjectModel;
using EventManagementSystem.Core.ViewModels;
using EventManagementSystem.Data.Model;

namespace EventManagementSystem.Models
{
    public class MailTemplateCategoryModel : ModelBase
    {
        #region Fields

        private readonly MailTemplateCategory _mailTemplateCategory;
        private ObservableCollection<MailTemplateModel> _templates;

        #endregion

        #region Properties

        public MailTemplateCategory MailTemplateCategory
        {
            get { return _mailTemplateCategory; }
        }

        public string Name
        {
            get { return _mailTemplateCategory.Name; }
            set
            {
                if (_mailTemplateCategory.Name == value) return;
                _mailTemplateCategory.Name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        public ObservableCollection<MailTemplateModel> Templates
        {
            get { return _templates; }
            set
            {
                if (_templates == value) return;
                _templates = value;
                RaisePropertyChanged(() => Templates);
            }
        }

        #endregion

        #region Constructor

        public MailTemplateCategoryModel(MailTemplateCategory category)
        {
            _mailTemplateCategory = category;
        }

        #endregion
    }
}
