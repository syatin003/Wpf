using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using EventManagementSystem.Core.Commands;
using EventManagementSystem.Core.Unity;
using EventManagementSystem.Data.Model;
using EventManagementSystem.Data.UnitOfWork.Interfaces;
using EventManagementSystem.Models;
using EventManagementSystem.ViewModels.Admin.Members.Age;
using EventManagementSystem.ViewModels.Admin.Members.Category;
using EventManagementSystem.ViewModels.Admin.Members.Group;
using EventManagementSystem.ViewModels.Admin.Members.LinkTypes;
using EventManagementSystem.ViewModels.Admin.Members.Style;
using EventManagementSystem.Views.Admin.Members.Age;
using EventManagementSystem.Views.Admin.Members.Category;
using EventManagementSystem.Views.Admin.Members.Group;
using EventManagementSystem.Views.Admin.Members.LinkTypes;
using EventManagementSystem.Views.Admin.Members.OptionBoxes;
using EventManagementSystem.Views.Admin.Members.Style;
using EventManagementSystem.Views.Admin.Members.Tokens;
using Telerik.Windows.Controls;
using ViewModelBase = EventManagementSystem.Core.ViewModels.ViewModelBase;
using Microsoft.Practices.Unity;

namespace EventManagementSystem.ViewModels.Admin.Members
{
    public class MembersViewModel : ViewModelBase
    {
        #region Fields

        private readonly IAdminDataUnit _adminDataUnit;
        private bool _isBusy;
        private ContentControl _content;
        private object _selectedTreeViewObject;
        private MembershipGroupStyleModel _membershipGroupStyle;
        private MembershipGroupAgeModel _membershipGroupAge;
        private MembershipGroupModel _membershipGroup;
        private string _treeViewItemPath;

        private MembershipCategoryModel _membershipCategory;
        private MembershipLinkTypeModel _membershipLinkType;

        private ObservableCollection<MembershipOptionBox> _membershipOptionBoxes;

        #endregion

        #region Properties

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        public ContentControl Content
        {
            get { return _content; }
            set
            {
                if (Equals(_content, value)) return;
                _content = value;
                RaisePropertyChanged(() => Content);
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
                if (_selectedTreeViewObject is MembershipOptionBox)
                {
                    var optionBox = value as MembershipOptionBox;
                    if (optionBox != null)
                        Content = new MembershipOptionBoxReasonsView(optionBox);
                }
                else
                    SetContentDescription();
                DeleteMembersPropertyCommand.RaiseCanExecuteChanged();
            }
        }

        public MembershipGroupStyleModel MembershipGroupStyle
        {
            get { return _membershipGroupStyle; }
            set
            {
                if (_membershipGroupStyle == value) return;
                _membershipGroupStyle = value;
                RaisePropertyChanged(() => MembershipGroupStyle);
            }
        }

        public MembershipGroupAgeModel MembershipGroupAge
        {
            get { return _membershipGroupAge; }
            set
            {
                if (_membershipGroupAge == value) return;
                _membershipGroupAge = value;
                RaisePropertyChanged(() => MembershipGroupAge);
            }
        }

        public MembershipGroupModel MembershipGroup
        {
            get { return _membershipGroup; }
            set
            {
                if (_membershipGroup == value) return;
                _membershipGroup = value;
                RaisePropertyChanged(() => MembershipGroup);
            }
        }

        public string TreeViewItemPath
        {
            get { return _treeViewItemPath; }
            set
            {
                if (_treeViewItemPath == value) return;
                _treeViewItemPath = value;
                RaisePropertyChanged(() => TreeViewItemPath);
            }
        }

        public MembershipCategoryModel MembershipCategory
        {
            get { return _membershipCategory; }
            set
            {
                if (_membershipCategory == value) return;
                _membershipCategory = value;
                RaisePropertyChanged(() => MembershipCategory);
            }
        }

        public MembershipLinkTypeModel MembershipLinkType
        {
            get { return _membershipLinkType; }
            set
            {
                if (_membershipLinkType == value) return;
                _membershipLinkType = value;
                RaisePropertyChanged(() => MembershipLinkType);
            }
        }

        public ObservableCollection<MembershipOptionBox> MembershipOptionBoxes
        {
            get { return _membershipOptionBoxes; }
            set
            {
                if (_membershipOptionBoxes == value) return;
                _membershipOptionBoxes = value;
                RaisePropertyChanged(() => MembershipOptionBoxes);
            }
        }

        public RelayCommand AddMembershipGroupStyleCommand { get; private set; }
        public RelayCommand AddMembershipGroupAgeCommand { get; private set; }
        public RelayCommand AddMembershipGroupCommand { get; private set; }
        public RelayCommand AddMembershipCategoryCommand { get; private set; }
        public RelayCommand AddMembershipLinkTypeCommand { get; private set; }
        public RelayCommand AddMembershipOptionBoxCommand { get; private set; }

        public RelayCommand DeleteMembersPropertyCommand { get; private set; }

        #endregion

        #region Constructor

        public MembersViewModel()
        {
            var dataUnitLocator = ContainerAccessor.Instance.GetContainer().Resolve<IDataUnitLocator>();
            _adminDataUnit = dataUnitLocator.ResolveDataUnit<IAdminDataUnit>();

            AddMembershipGroupStyleCommand = new RelayCommand(AddMembershipGroupStyleCommandExecuted);
            AddMembershipGroupAgeCommand = new RelayCommand(AddMembershipGroupAgeCommandExecuted);
            AddMembershipGroupCommand = new RelayCommand(AddMembershipGroupCommandExecuted);
            AddMembershipCategoryCommand = new RelayCommand(AddMembershipCategoryCommandExecuted);
            AddMembershipLinkTypeCommand = new RelayCommand(AddMembershipLinkTypeCommandExecuted);
            AddMembershipOptionBoxCommand = new RelayCommand(AddMembershipOptionBoxCommandExecuted);

            DeleteMembersPropertyCommand = new RelayCommand(DeleteMembersPropertyCommandExecuted, DeleteMembersPropertyCommandCanExecute);

        }

        #endregion

        #region Methods


        public async void LoadData()
        {
            IsBusy = true;

            _adminDataUnit.MembershipOptionBoxesRepository.Refresh();

            var optionBoxes = await _adminDataUnit.MembershipOptionBoxesRepository.GetAllAsync();
            MembershipOptionBoxes = new ObservableCollection<MembershipOptionBox>(optionBoxes);

            IsBusy = false;
        }

        private void SetContentDescription()
        {
            if (_selectedTreeViewObject != null)
                switch (_selectedTreeViewObject.ToString())
                {
                    case "Members":
                        Content = null;
                        break;
                    case "Category Groups":
                        Content = null;
                        break;
                    case "Style":
                        Content = new MembershipGroupStylesView();
                        break;
                    case "Age":
                        Content = new MembershipGroupAgesView();
                        break;
                    case "Groups":
                        Content = new MembershipGroupsView();
                        break;
                    case "Categories":
                        Content = new MembershipCategoriesView();
                        break;
                    case "Tokens":
                        Content = new MembershipTokensView();
                        break;
                    case "Link Types":
                        Content = new MembershipLinkTypesView();
                        break;
                }
            else
                Content = null;
        }

        #endregion Methods

        #region Commands

        private void AddMembershipGroupStyleCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var addMembershipGroupStyleView = new AddMembershipGroupStyleView(MembershipGroupStyle);
            addMembershipGroupStyleView.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");
            if (addMembershipGroupStyleView.DialogResult != null && addMembershipGroupStyleView.DialogResult == true)
            {
                if (Content != null)
                {
                    var viewModel = Content.DataContext as MembershipGroupStylesViewModel;
                    if (viewModel != null)
                    {
                        viewModel.AllMembershipGroupStyles.Add(addMembershipGroupStyleView.ViewModel.MembershipGroupStyle);
                        viewModel.RefreshMembershipGroupStyles();
                    }
                }
                TreeViewItemPath = "Members|Category Groups|Style";

                RaisePropertyChanged("SelectTreeViewItem");
            }
        }
        private void AddMembershipGroupAgeCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var addMembershipGroupAgeView = new AddMembershipGroupAgeView(MembershipGroupAge);
            addMembershipGroupAgeView.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");
            if (addMembershipGroupAgeView.DialogResult != null && addMembershipGroupAgeView.DialogResult == true)
            {
                if (Content != null)
                {
                    var viewModel = Content.DataContext as MembershipGroupAgesViewModel;
                    if (viewModel != null)
                    {
                        viewModel.AllMembershipGroupAges.Add(addMembershipGroupAgeView.ViewModel.MembershipGroupAge);
                        viewModel.RefreshMembershipGroupAges();
                    }
                }
                TreeViewItemPath = "Members|Category Groups|Age";

                RaisePropertyChanged("SelectTreeViewItem");
            }
        }

        private void AddMembershipGroupCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var addMembershipGroupView = new AddMembershipGroupView(MembershipGroup);
            addMembershipGroupView.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");
            if (addMembershipGroupView.DialogResult != null && addMembershipGroupView.DialogResult == true)
            {
                if (Content != null)
                {
                    var viewModel = Content.DataContext as MembershipGroupsViewModel;
                    if (viewModel != null)
                    {
                        viewModel.AllMembershipGroups.Add(addMembershipGroupView.ViewModel.MembershipGroup);
                        viewModel.RefreshMembershipGroups();
                    }
                }
                TreeViewItemPath = "Members|Category Groups|Groups";

                RaisePropertyChanged("SelectTreeViewItem");
            }
        }
        private void AddMembershipCategoryCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var addMembershipCategoryView = new AddMembershipCategoryView(MembershipCategory);
            addMembershipCategoryView.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");
            if (addMembershipCategoryView.DialogResult != null && addMembershipCategoryView.DialogResult == true)
            {
                if (Content != null)
                {
                    var viewModel = Content.DataContext as MembershipCategoriesViewModel;
                    if (viewModel != null)
                    {
                        viewModel.AllMembershipCategories.Add(addMembershipCategoryView.ViewModel.MembershipCategory);
                        viewModel.RefreshMembershipCategories();
                        viewModel.SelectedMembershipCategory = addMembershipCategoryView.ViewModel.MembershipCategory;
                    }
                }

                TreeViewItemPath = "Members|Categories";

                RaisePropertyChanged("SelectTreeViewItem");
            }
        }

        private void AddMembershipLinkTypeCommandExecuted()
        {
            RaisePropertyChanged("DisableParentWindow");

            var addMembershipLinkTypeView = new AddMembershipLinkTypeView(MembershipLinkType);
            addMembershipLinkTypeView.ShowDialog();

            RaisePropertyChanged("EnableParentWindow");
            if (addMembershipLinkTypeView.DialogResult != null && addMembershipLinkTypeView.DialogResult == true)
            {
                if (Content != null)
                {
                    var viewModel = Content.DataContext as MembershipLinkTypesViewModel;
                    if (viewModel != null)
                    {
                        viewModel.AllMembershipLinkTypes.Add(addMembershipLinkTypeView.ViewModel.MembershipLinkType);
                        viewModel.RefreshMembershipLinkTypes();
                        viewModel.SelectedMembershipLinkType = addMembershipLinkTypeView.ViewModel.MembershipLinkType;
                    }
                }

                TreeViewItemPath = "Members|Link Types";

                RaisePropertyChanged("SelectTreeViewItem");
            }
        }

        private void AddMembershipOptionBoxCommandExecuted()
        {
            string name = string.Empty;

            RaisePropertyChanged("DisableParentWindow");

            RadWindow.Prompt(new DialogParameters()
            {
                Header = "Add Option Box",
                Closed = (sender, args) => { name = args.PromptResult; }
            });

            RaisePropertyChanged("EnableParentWindow");

            if (!string.IsNullOrWhiteSpace(name))
            {
                var membershipOptionBox = new MembershipOptionBox()
                {
                    ID = Guid.NewGuid(),
                    Name = name
                };
                _adminDataUnit.MembershipOptionBoxesRepository.Add(membershipOptionBox);
                _adminDataUnit.SaveChanges();

                MembershipOptionBoxes.Add(membershipOptionBox);

                TreeViewItemPath = "Members|Option Boxes";

                RaisePropertyChanged("SelectTreeViewItem");

                SelectedTreeViewObject = membershipOptionBox;
            }
        }

        private bool DeleteMembersPropertyCommandCanExecute()
        {
            return (SelectedTreeViewObject is MembershipOptionBox);
        }

        private void DeleteMembersPropertyCommandExecuted()
        {
            if (SelectedTreeViewObject == null) return;

            bool? dialogResult = null;

            string confirmText = Properties.Resources.MESSAGE_ASK_BEFORE_DELETING_ITEM;

            RaisePropertyChanged("DisableParentWindow");

            RadWindow.Confirm(confirmText, (sender, args) => { dialogResult = args.DialogResult; });

            RaisePropertyChanged("EnableParentWindow");

            if (dialogResult != true) return;

            if (SelectedTreeViewObject is MembershipOptionBox)
            {
                var optionBox = SelectedTreeViewObject as MembershipOptionBox;

                //delete Membership Option Box Options
                if (optionBox.MembershipOptionBoxReasons.Any())
                    _adminDataUnit.MembershipOptionBoxReasonsRepository.Delete(optionBox.MembershipOptionBoxReasons.ToList());

                // delete Membership Option Box
                _adminDataUnit.MembershipOptionBoxesRepository.Delete(optionBox);
                _adminDataUnit.SaveChanges();

                MembershipOptionBoxes.Remove(optionBox);
            }
        }

        #endregion Commands
    }
}
